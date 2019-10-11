using ConsoleApp2.Components;
using ConsoleApp2.Core;
using LibraProgramming.Ecs;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Extensions;

namespace ConsoleApp2.Systems
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateEnemiesSystem : SystemBase, IDisposable
    {
        private readonly IGameTimer timer;
        private readonly IWorld world;
        private LiveComponentObserver<UpdateComponent> observer;
        private IDisposable disposable;

        [PrefferedConstructor]
        public UpdateEnemiesSystem(IWorld world, IGameTimer timer)
        {
            this.timer = timer;
            this.world = world;
        }

        public override Task InitializeAsync()
        {
            observer = world.Root.Subscribe<UpdateComponent>("//CurrentWave/Enemies/*");
            disposable = timer.Subscribe(DoUpdateComponents);

            return Task.CompletedTask;
        }

        public override Task ExecuteAsync(CancellationToken cancellationToken) => cancellationToken.AsTask();

        public void Dispose()
        {
            disposable.Dispose();
            observer.Dispose();
        }

        private void DoUpdateComponents(TimeSpan elapsed)
        {
            foreach (var component in observer)
            {
                //Debug.WriteLine($"[UpdateSystem.DoUpdateComponent] (\'{component.Entity.Key}\') Set UpdateComponent.Elapsed = {elapsed:g}");
                component.Elapsed = elapsed;
            }
        }
    }
}