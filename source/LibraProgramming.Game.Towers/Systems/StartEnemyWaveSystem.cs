using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Extensions;
using LibraProgramming.Game.Towers.Components;
using LibraProgramming.Game.Towers.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.Game.Towers.Systems
{
    public class StartEnemyWaveSystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IGameTimer timer;
        private readonly IEnemyMoveStrategy moveStrategy;
        private readonly ILogger logger;
        private LiveComponentObserver observer;
        private IDisposable disposable;
        private EntityBase container;

        [PrefferedConstructor]
        public StartEnemyWaveSystem(
            IWorld world,
            IGameTimer timer,
            IEnemyMoveStrategy moveStrategy,
            ILogger logger)
        {
            this.world = world;
            this.timer = timer;
            this.moveStrategy = moveStrategy;
            this.logger = logger;
        }

        public override Task InitializeAsync()
        {
            observer = world.Root.Subscribe<DelayComponent>("//CurrentWave/Enemies/*");
            container = world.Root.Find("//Scene/Enemies");
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
            foreach (var group in observer)
            {
                var delayComponent = group.Get<DelayComponent>();

                if (TimeSpan.Zero < delayComponent.Duration)
                {
                    delayComponent.Duration -= elapsed;
                    continue;
                }

                PresentEnemiesToScene(group.Children);

                var parent = group.Parent;

                parent.Children.Remove(group);
            }
        }

        private void PresentEnemiesToScene(IEnumerable<EntityBase> enemies)
        {
            var children = enemies.ToArray();

            foreach (var child in children)
            {
                moveStrategy.PlaceEnemy(child);
                container.Children.Add(child);

                logger.LogDebug($"Adding enemy \'{child.Key}\'");
            }
        }
    }
}