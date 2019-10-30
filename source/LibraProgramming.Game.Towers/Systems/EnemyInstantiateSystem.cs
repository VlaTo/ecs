using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Extensions;
using LibraProgramming.Game.Towers.Components;
using LibraProgramming.Game.Towers.Core;

namespace LibraProgramming.Game.Towers.Systems
{
    public class EnemyInstantiateSystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IGameTimer timer;
        private LiveComponentObserver observer;
        private IDisposable disposable;
        private EntityBase currentWaveEnemies;

        [PrefferedConstructor]
        public EnemyInstantiateSystem(IWorld world, IGameTimer timer)
        {
            this.world = world;
            this.timer = timer;
        }

        public override Task InitializeAsync()
        {
            observer = world.Root.Subscribe<DelayComponent>("//CurrentWave/Portions/*");
            currentWaveEnemies = world.Root.Find("//CurrentWave/Enemies");
            disposable = timer.Subscribe(DoUpdate);

            return Task.CompletedTask;
        }

        public override Task ExecuteAsync(CancellationToken cancellationToken) => cancellationToken.AsTask();

        public void Dispose()
        {
            disposable.Dispose();
            observer.Dispose();
        }
        
        private void DoUpdate(TimeSpan elapsed)
        {
            foreach (var portion in observer)
            {
                var component = portion.Get<DelayComponent>();

                if (TimeSpan.Zero < component.Duration)
                {
                    component.Duration -= elapsed;
                    continue;
                }

                var children = portion.Children.ToArray();

                foreach (var enemy in children)
                {
                    currentWaveEnemies.Children.Add(enemy);
                }

                var collection = portion.Parent.Children;

                collection.Remove(portion);
            }
        }
    }
}