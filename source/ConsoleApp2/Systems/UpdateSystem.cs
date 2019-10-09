using System;
using System.Threading;
using System.Threading.Tasks;
using ClassLibrary1;
using ClassLibrary1.Core;
using ClassLibrary1.Extensions;
using ConsoleApp2.Components;
using ConsoleApp2.Core;

namespace ConsoleApp2.Systems
{

    public class UpdateSystem : SystemBase, IDisposable
    {
        private readonly IGameTimer timer;
        private IWorld world;
        //private IDisposable timerSubscription;
        //private IComponentSubscription<UpdateComponent> components;

        public UpdateSystem(IGameTimer timer)
        {
            this.timer = timer;
        }

        public override Task InitializeAsync(IWorld world)
        {
            this.world = world;

            //timerSubscription = timer.Subscribe(DoUpdate);
            //componentSubscription = world.Root.Subscribe<UpdateComponent>("//*");

            return Task.CompletedTask;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var components = world.Root.Subscribe<UpdateComponent>("//*"))
            {
                using (timer.Subscribe(elapsed => DoUpdate(components, elapsed)))
                {
                    try
                    {
                        await Task.FromCanceled(cancellationToken);
                    }
                    finally
                    {
                        ;
                    }
                }
            }
        }

        public void Dispose()
        {
            //components.Dispose();
            //timerSubscription.Dispose();
        }

        private void DoUpdate(IComponentEnumerable<UpdateComponent> components, TimeSpan elapsed)
        {
            foreach (var component in components)
            {
                component.Elapsed.Value = elapsed;
            }
        }
    }
}