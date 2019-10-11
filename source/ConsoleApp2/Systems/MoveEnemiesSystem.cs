using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp2.Components;
using ConsoleApp2.Core;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Extensions;

namespace ConsoleApp2.Systems
{
    public sealed class MoveEnemiesSystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IGameTimer timer;
        private TimeSpan lastElapsed;
        private LiveComponentObserver<MoveComponent> observer;
        private IDisposable disposable;

        [PrefferedConstructor]
        public MoveEnemiesSystem(IWorld world, IGameTimer timer)
        {
            this.world = world;
            this.timer = timer;
            lastElapsed = TimeSpan.Zero;
        }

        public override Task InitializeAsync()
        {
            lastElapsed = TimeSpan.Zero;
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
                var duration = (float) (elapsed - lastElapsed).TotalSeconds;

                foreach (var component in observer)
                {
                    var distance = Vector2.Multiply(component.Speed, duration);
                    var position = component.Position + distance;
                    var viewport = GetViewport(component.Entity);

                    if (null != viewport)
                    {
                        if (position.X > viewport.Width)
                        {
                            position.X = 0.0f;
                        }

                        if (position.Y > viewport.Height)
                        {
                            position.Y = 0.0f;
                        }
                    }

                    Debug.WriteLine($"[MoveEnemiesSystem.DoUpdateComponents] Move \'{component.Entity.Key}\': ({component.Position.X:F1};{component.Position.Y:F1}) => ({position.X:F1};{position.Y:F1})");
                    
                    component.Position = position;
                }
            }
            finally
            {
                lastElapsed = elapsed;
            }
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