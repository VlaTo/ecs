using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary1.Core;

namespace ClassLibrary1
{
    public sealed class World : IWorld
    {
        private readonly IDependencyInjection dependencyInjection;
        private readonly List<Type> systemTypes;

        public EntityBase Root { get; }

        public World(IDependencyInjection dependencyInjection)
        {
            this.dependencyInjection = dependencyInjection;
            systemTypes = new List<Type>();

            Root = new Entity(nameof(Root));
        }

        public void RegisterSystem<TSystem>()
            where TSystem : ISystem
        {
            var systemType = typeof(TSystem);

            if (systemTypes.Contains(systemType))
            {
                throw new InvalidOperationException();
            }

            dependencyInjection.Register(systemType);
            systemTypes.Add(systemType);
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var systems = new List<ISystem>();

            foreach (var systemType in systemTypes)
            {
                var system = dependencyInjection.GetService(systemType) as ISystem;

                if (null == system)
                {
                    throw new InvalidCastException();
                }

                systems.Add(system);
            }

            await ExecuteSystems(systems, system => system.InitializeAsync(this));

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
                    await ExecuteSystems(systems, system =>
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

        private static Task ExecuteSystems(IList<ISystem> systems, Func<ISystem, Task> action)
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