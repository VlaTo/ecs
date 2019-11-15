using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Core.Path;
using LibraProgramming.Ecs.Extensions;
using LibraProgramming.Game.Towers.Components;
using LibraProgramming.Game.Towers.Core;
using Microsoft.Extensions.Logging;

namespace LibraProgramming.Game.Towers.Systems
{
    public class EnemyWaveSystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IGameTimer timer;
        private readonly IEnemyMoveStrategy enemyMove;
        private readonly ILogger logger;
        private readonly EntityPath pathNodePath;
        private LiveComponentObserver observer;
        private IDisposable disposable;
        private EntityBase container;

        [PrefferedConstructor]
        public EnemyWaveSystem(
            IWorld world,
            IGameTimer timer,
            IEnemyMoveStrategy enemyMove,
            ILogger logger)
        {
            this.world = world;
            this.timer = timer;
            this.enemyMove = enemyMove;
            this.logger = logger;
            pathNodePath = "../../../Path";
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

                PresentEnemyGroupToScene(group);

                var parent = group.Parent;

                parent.Children.Remove(group);
            }
        }

        private void PresentEnemyGroupToScene(EntityBase enemies)
        {
            var children = enemies.Children.ToArray();

            foreach (var child in children)
            {
                var points = child.Find(pathNodePath);
                var pathComponent = points.Get<PathComponent>();
                var positionComponent = child.Get<PositionComponent>();
                var moveComponent = child.Get<MoveComponent>();

                if (null == positionComponent)
                {
                    positionComponent = new PositionComponent();
                    child.Add(positionComponent);
                }

                if (null == moveComponent)
                {
                    moveComponent = new MoveComponent();
                    child.Add(moveComponent);
                }

                positionComponent.Position = enemyMove.GetOrigin(pathComponent);
                //moveComponent.Angle = positionProvider.CalculateAngle(origin, Vector2.One);

                container.Children.Add(child);
                logger.LogDebug($"Adding enemy \'{child.Key}\'");
            }
        }
    }
}