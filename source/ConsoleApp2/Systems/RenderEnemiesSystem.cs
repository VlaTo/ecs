using System;
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
    public sealed class RenderEnemiesSystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IRenderer renderer;
        private LiveComponentObserver<RenderComponent> observer;
        private IDisposable disposable;

        [PrefferedConstructor]
        public RenderEnemiesSystem(IWorld world, IRenderer renderer)
        {
            this.world = world;
            this.renderer = renderer;
        }

        public override Task InitializeAsync()
        {
            disposable = renderer.Subscribe(DoRender);
            observer = world.Root.Subscribe<RenderComponent>("//CurrentWave/Enemies/*");

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

        private void DoRender(IRenderContext context)
        {
            foreach (var component in observer)
            {
                var moveComponent = component.Entity.Get<MoveComponent>();

                if (null == moveComponent)
                {
                    continue;
                }

                var transform = GetTransform(component.Entity);
                var position = Vector2.Transform(moveComponent.Position, transform);

                ;
            }
        }

        private static Matrix3x2 GetTransform(EntityBase entity)
        {
            var parent = entity;
            var transform = Matrix3x2.Identity;

            while (null != parent)
            {
                if (parent.Has<TransformComponent>())
                {
                    var transformComponent = parent.Get<TransformComponent>();
                    transform = Matrix3x2.Multiply(transform, transformComponent.Transform);
                }

                parent = parent.Parent;
            }

            return transform;
        }
    }
}