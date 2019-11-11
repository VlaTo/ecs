using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Extensions;
using LibraProgramming.Game.Towers.Components;
using LibraProgramming.Game.Towers.Core;
using System;
using System.Composition;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.Game.Towers.Systems
{
    [Export(typeof(ISystem))]
    [ExportMetadata("Type", nameof(MoveEntitySystem))]
    public sealed class MoveEntitySystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IGameTimer timer;
        private LiveComponentObserver observer;
        private IDisposable disposable;

        [ImportingConstructor]
        [PrefferedConstructor]
        public MoveEntitySystem(IWorld world, IGameTimer timer)
        {
            this.world = world;
            this.timer = timer;
        }

        public override Task InitializeAsync()
        {
            disposable = timer.Subscribe(DoUpdateComponents);
            observer = world.Root.Subscribe<PositionComponent, MoveComponent>("//Scene/**/*");

            return base.InitializeAsync();
        }

        public override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return cancellationToken.AsTask();
        }

        public void Dispose()
        {
            disposable.Dispose();
            observer.Dispose();
        }

        private void DoUpdateComponents(TimeSpan elapsed)
        {
            try
            {
                var duration = (float) elapsed.TotalSeconds;

                foreach (var entity in observer)
                {
                    var position = entity.Get<PositionComponent>();
                    var move = entity.Get<MoveComponent>();
                    UpdatePosition(position, move, duration);
                }
            }
            finally
            {
                ;
            }
        }

        private static void UpdatePosition(PositionComponent positionComponent, MoveComponent moveComponent, float duration)
        {
            var distance = new Vector2(MathF.Cos(moveComponent.Angle), MathF.Sin(moveComponent.Angle)) * (moveComponent.Speed * duration);
            var position = positionComponent.Position + distance;
            var viewport = GetViewport(moveComponent.Entity);

            if (null != viewport)
            {
                if (position.X < viewport.Horizontal.Min)
                {
                    position.X = viewport.Horizontal.Max;
                }
                else if (position.X > viewport.Horizontal.Max)
                {
                    position.X = viewport.Horizontal.Min;
                }

                if (position.Y < viewport.Vertical.Min)
                {
                    position.Y = viewport.Vertical.Max;
                }
                else if (position.Y > viewport.Vertical.Max)
                {
                    position.Y = viewport.Vertical.Min;
                }
            }

            positionComponent.Position = position;
        }

        private static ViewportComponent GetViewport(EntityBase entity)
        {
            var parent = entity;

            while (null != parent)
            {
                if (parent.Has<ViewportComponent>())
                {
                    return parent.Get<ViewportComponent>();
                }

                parent = parent.Parent;
            }

            return null;
        }
    }
}