using System;
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
    /// <summary>
    /// 
    /// </summary>
    public class UpdateEnemiesSystem : SystemBase, IDisposable
    {
        private readonly IGameTimer timer;
        private readonly IEnemyMoveStrategy moveStrategy;
        private readonly IWorld world;
        private LiveComponentObserver observer;
        private IDisposable disposable;

        [PrefferedConstructor]
        public UpdateEnemiesSystem(
            IWorld world,
            IGameTimer timer,
            IEnemyMoveStrategy moveStrategy)
        {
            this.world = world;
            this.timer = timer;
            this.moveStrategy = moveStrategy;
        }

        public override Task InitializeAsync()
        {
            observer = world.Root.Subscribe<UpdateComponent>("//Scene/Enemies/*");
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
            foreach (var entity in observer)
            {
                moveStrategy.MoveEnemy(entity, elapsed);
            }
        }
    }
}