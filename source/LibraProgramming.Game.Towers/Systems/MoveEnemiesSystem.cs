using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Extensions;
using LibraProgramming.Game.Towers.Components;
using LibraProgramming.Game.Towers.Core;

namespace LibraProgramming.Game.Towers.Systems
{
    public sealed class MoveEnemiesSystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IGameTimer timer;
        private LiveComponentObserver observer;
        private IDisposable disposable;

        [PrefferedConstructor]
        public MoveEnemiesSystem(IWorld world, IGameTimer timer)
        {
            this.world = world;
            this.timer = timer;
        }

        public override Task InitializeAsync()
        {
            disposable = timer.Subscribe(DoUpdateComponents);
            observer = world.Root.Subscribe<MoveComponent>("//CurrentWave/Enemies/*");

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
                    var component = entity.Get<MoveComponent>();
                    UpdatePosition(component, duration);
                }
            }
            finally
            {
                ;
            }
        }

        private static void UpdatePosition(MoveComponent component, float duration)
        {
            var distance = new Vector2(MathF.Cos(component.Angle), MathF.Sin(component.Angle)) * (component.Speed * duration);
            var position = component.Position + distance;
            var viewport = GetViewport(component.Entity);

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

            component.Position = position;
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