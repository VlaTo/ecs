﻿using System;
using System.Composition;
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
    [Export(typeof(ISystem))]
    [ExportMetadata("Type", nameof(RenderEnemiesSystem))]
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
            observer = world.Root.Subscribe<RenderComponent, PositionComponent>("//Scene/Enemies/*");

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
                var positionComponent = entity.Get<PositionComponent>();
                var transform = GetTransform(entity);
                var position = Vector2.Transform(positionComponent.Position, transform);

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