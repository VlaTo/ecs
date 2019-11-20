using System;
using System.Composition;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Core.Reactive;
using LibraProgramming.Ecs.Extensions;
using LibraProgramming.Game.Towers.Components;
using LibraProgramming.Game.Towers.Core;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using ObservableExtensions = LibraProgramming.Ecs.Extensions.ObservableExtensions;

namespace LibraProgramming.Game.Towers.Systems
{
    [Export(typeof(ISystem))]
    [ExportMetadata("Type", nameof(RenderEnemiesSystem))]
    public sealed class RenderEnemiesSystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IGameRenderer gameRenderer;
        private readonly IGameResourcesCreator resourcesCreator;
        private LiveComponentObserver observer;
        private IDisposable disposable;
        private ICanvasBrush layerBrush;
        private CanvasBitmap enemySprite;

        [PrefferedConstructor]
        public RenderEnemiesSystem(
            IWorld world,
            IGameRenderer gameRenderer,
            IGameResourcesCreator resourcesCreator)
        {
            this.world = world;
            this.gameRenderer = gameRenderer;
            this.resourcesCreator = resourcesCreator;
        }

        public override Task InitializeAsync()
        {
            disposable = new CompositeDisposable(
                gameRenderer.Subscribe(DoRender),
                resourcesCreator.Subscribe(DoCreateResourcesAsync)
            );
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

        private void DoRender(IGameRenderContext renderContext)
        {
            using (renderContext.DrawingSession.CreateLayer(layerBrush))
            {
                using (var batch = renderContext.DrawingSession.CreateSpriteBatch(CanvasSpriteSortMode.Bitmap))
                {
                    foreach (var entity in observer)
                    {
                        var positionComponent = entity.Get<PositionComponent>();
                        var transform = GetTransform(entity);
                        var position = Vector2.Transform(positionComponent.Position, transform);

                        batch.Draw(enemySprite, position);
                    }
                }
            }
        }

        private async Task DoCreateResourcesAsync(ICreateResourcesContext context)
        {
            layerBrush = new CanvasSolidColorBrush(context.Creator, Colors.White);
            enemySprite = await CreateEnemySpriteAsync(context.Creator);
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

        private Task<CanvasBitmap> CreateEnemySpriteAsync(ICanvasResourceCreatorWithDpi resourceCreator)
        {
            using (var renderTarget = new CanvasRenderTarget(resourceCreator, 20.0f, 20.0f))
            {
                using (var session = renderTarget.CreateDrawingSession())
                {
                    var origin = new Vector2(10.0f, 10.0f);

                    session.Clear(Colors.Transparent);

                    using (var outline = new CanvasSolidColorBrush(session, Colors.Gray))
                    {
                        session.FillCircle(origin, 8.0f, outline);
                        session.DrawCircle(origin, 8.0f, Colors.AntiqueWhite, 0.4f);
                    }
                }

                var size = renderTarget.SizeInPixels;
                var sprite = CanvasBitmap.CreateFromBytes(
                    resourceCreator,
                    renderTarget.GetPixelBytes(),
                    (int) size.Width,
                    (int) size.Height,
                    renderTarget.Format
                );

                return Task.FromResult(sprite);
            }
        }
    }
}