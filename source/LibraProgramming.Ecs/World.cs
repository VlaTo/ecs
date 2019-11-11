using LibraProgramming.Ecs.Core;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    [Export(typeof(IWorld))]
    public sealed class World : IWorld
    {
        private readonly IDependencyProvider dependencyProvider;
        private readonly List<Type> systemTypes;

        /// <inheritdoc cref="IWorld.Root" />
        public Entity Root
        {
            get;
        }

        //[ImportMany]
        public IList<ISystem> Systems
        {
            get;
            set;
        }

        /*[ImportingConstructor]
        public World()
        {
            Systems = new List<ISystem>();
            Root = new Entity(nameof(Root));
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependencyProvider"></param>
        public World(IDependencyProvider dependencyProvider)
        {
            this.dependencyProvider = dependencyProvider;
            systemTypes = new List<Type>();

            Systems = new List<ISystem>();
            Root = new Entity(nameof(Root));
        }

        /// <inheritdoc cref="IWorld.RegisterSystem{TSystem}" />
        public void RegisterSystem<TSystem>() where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (systemTypes.Exists(type => systemType == type))
            {
                throw new ArgumentException("", "TSystem");
            }

            systemTypes.Add(systemType);
        }

        /// <inheritdoc cref="IWorld.ExecuteAsync" />
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var systems = new List<ISystem>();

            foreach (var systemType in systemTypes)
            {
                var system = dependencyProvider.CreateSystem(systemType);
                systems.Add(system);
            }

            await InvokeSystems(systems, system => system.InitializeAsync());

            try
            {
                var cancellationTokens = new Dictionary<ISystem, CancellationTokenSource>();

                void CancellationRequested()
                {
                    foreach (var kvp in cancellationTokens)
                    {
                        kvp.Value.Cancel(false);
                    }
                }

                using (cancellationToken.Register(CancellationRequested))
                {
                    await InvokeSystems(systems, system =>
                    {
                        var cts = new CancellationTokenSource();

                        cancellationTokens[system] = cts;

                        return system.ExecuteAsync(cts.Token);
                    });
                }
            }
            finally
            {
                var disposables = systems.OfType<IDisposable>();

                foreach (var disposable in disposables)
                {
                    disposable.Dispose();
                }
            }
        }

        private static Task InvokeSystems(IReadOnlyList<ISystem> systems, Func<ISystem, Task> action)
        {
            var tasks = new Task[systems.Count];

            for (var index = 0; index < systems.Count; index++)
            {
                tasks[index] = action.Invoke(systems[index]);
            }

            return Task.WhenAll(tasks);
        }
    }
}