using System;
using System.Numerics;
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
    public sealed class RenderEnemiesSystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IGameRenderer gameRenderer;
        private LiveComponentObserver observer;
        private IDisposable disposable;

        [PrefferedConstructor]
        public RenderEnemiesSystem(IWorld world, IGameRenderer gameRenderer)
        {
            this.world = world;
            this.gameRenderer = gameRenderer;
        }

        public override Task InitializeAsync()
        {
            disposable = gameRenderer.Subscribe(DoRender);
            observer = world.Root.Subscribe<RenderComponent, MoveComponent>("//Scene/Enemies/*");

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

        private void DoRender(IGameRenderContext gameRenderContext)
        {
            foreach (var entity in observer)
            {
                var moveComponent = entity.Get<MoveComponent>();
                var transform = GetTransform(entity);
                var position = Vector2.Transform(moveComponent.Position, transform);

                gameRenderContext.RenderEnemy(position);
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