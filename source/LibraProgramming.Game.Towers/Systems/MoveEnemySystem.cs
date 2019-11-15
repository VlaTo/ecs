using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Extensions;
using LibraProgramming.Game.Towers.Components;
using LibraProgramming.Game.Towers.Core;
using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LibraProgramming.Game.Towers.Systems
{
    [Export(typeof(ISystem))]
    [ExportMetadata("Type", nameof(MoveEnemySystem))]
    public sealed class MoveEnemySystem : SystemBase
    {
        private readonly IWorld world;
        private readonly IGameTimer timer;
        private readonly IEnemyMoveStrategy moveStrategy;
        private readonly ILogger logger;
        private LiveComponentObserver observer;
        private IDisposable disposable;

        [ImportingConstructor]
        [PrefferedConstructor]
        public MoveEnemySystem(
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
            disposable = timer.Subscribe(DoUpdateComponents);
            observer = world.Root.Subscribe<PositionComponent, WayPointComponent>("//Scene/Enemies/*");

            return base.InitializeAsync();
        }

        public override Task ExecuteAsync(CancellationToken cancellationToken) => cancellationToken.AsTask();

        public void Dispose()
        {
            disposable.Dispose();
            observer.Dispose();
        }

        private void DoUpdateComponents(TimeSpan elapsed)
        {
            try
            {
                foreach (var entity in observer)
                {
                    moveStrategy.Move(entity, elapsed);

                    /*var position = entity.Get<PositionComponent>();
                    var move = entity.Get<MoveComponent>();
                    UpdatePosition(position, move, duration);*/
                }
            }
            finally
            {
                ;
            }
        }
    }
}