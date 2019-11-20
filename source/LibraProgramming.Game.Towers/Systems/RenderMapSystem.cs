using System;
using System.Composition;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Core.Reactive;
using LibraProgramming.Ecs.Extensions;
using LibraProgramming.Game.Towers.Components;
using LibraProgramming.Game.Towers.Core;
using LibraProgramming.Game.Towers.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace LibraProgramming.Game.Towers.Systems
{
    [Export(typeof(ISystem))]
    [ExportMetadata("Type", nameof(RenderMapSystem))]
    public class RenderMapSystem : SystemBase, IDisposable
    {
        private readonly IWorld world;
        private readonly IGameRenderer gameRenderer;
        private readonly IGameResourcesCreator resourcesCreator;
        private readonly IFileProvider fileProvider;
        private readonly ILogger logger;
        private LiveComponentObserver observer;
        private IDisposable disposable;
        private CanvasBitmap spriteSheet;
        private CanvasBitmap mapBitmap;

        [ImportingConstructor]
        [PrefferedConstructor]
        public RenderMapSystem(
            IWorld world,
            IGameRenderer gameRenderer,
            IGameResourcesCreator resourcesCreator,
            IFileProvider fileProvider,
            ILogger logger)
        {
            this.world = world;
            this.gameRenderer = gameRenderer;
            this.resourcesCreator = resourcesCreator;
            this.fileProvider = fileProvider;
            this.logger = logger;
        }

        public override Task InitializeAsync()
        {
            disposable = new CompositeDisposable(
                gameRenderer.Subscribe(DoRender),
                resourcesCreator.Subscribe(DoCreateResourcesAsync)
            );
            observer = world.Root.Subscribe<MapComponent, SpriteSheetComponent>("//Scene/Map");

            return base.InitializeAsync();
        }

        public override Task ExecuteAsync(CancellationToken cancellationToken) => cancellationToken.AsTask();

        public void Dispose()
        {
            disposable.Dispose();
            observer.Dispose();
        }

        private void DoRender(IGameRenderContext renderContext)
        {
            renderContext.DrawingSession.DrawImage(mapBitmap);
        }

        private async Task DoCreateResourcesAsync(ICreateResourcesContext context)
        {
            foreach (var entity in observer)
            {
                var spriteSheetComponent = entity.Get<SpriteSheetComponent>();
                var mapComponent = entity.Get<MapComponent>();

                spriteSheet = await CreateSpriteSheetAsync(context.Creator, spriteSheetComponent.Source);
                mapBitmap = await CreateMapAsync(context.Creator, context.CanvasSize, mapComponent, spriteSheetComponent);
            }
        }

        private async Task<CanvasBitmap> CreateSpriteSheetAsync(ICanvasResourceCreatorWithDpi creator, string source)
        {
            using (var file = fileProvider.GetFile(source))
            {
                using (var stream = file.OpenRead())
                {
                    return await CanvasBitmap.LoadAsync(creator, stream.AsRandomAccessStream());
                }
            }
        }

        private Task<CanvasBitmap> CreateMapAsync(
            ICanvasResourceCreatorWithDpi creator,
            Size canvasSize,
            MapComponent mapComponent,
            SpriteSheetComponent spriteSheetComponent)
        {
            using (var renderTarget = new CanvasRenderTarget(creator, canvasSize))
            {
                var size = renderTarget.SizeInPixels;

                using (var session = renderTarget.CreateDrawingSession())
                {
                    using (var batch = session.CreateSpriteBatch())
                    {
                        TileMap(batch, size, mapComponent, spriteSheetComponent);
                    }
                }

                var sprite = CanvasBitmap.CreateFromBytes(
                    creator,
                    renderTarget.GetPixelBytes(),
                    (int) size.Width,
                    (int) size.Height,
                    renderTarget.Format,
                    renderTarget.Dpi
                );

                return Task.FromResult(sprite);
            }
        }

        private void TileMap(
            CanvasSpriteBatch batch,
            BitmapSize bitmapSize,
            MapComponent mapComponent,
            SpriteSheetComponent spriteSheetComponent)
        {
            //var tileSize = spriteSheetComponent.TileSize;
            var sourceRect = new Rect(new Point(), spriteSheetComponent.TileSize.ToSize());
            var mapTileSize = new Vector2(
                ((float) bitmapSize.Width) / mapComponent.Size.Width,
                ((float) bitmapSize.Height) / mapComponent.Size.Height
            );
            var scale = mapTileSize / spriteSheetComponent.TileSize.ToVector2();

            for (var y = 0.0f; y < bitmapSize.Height; y += mapTileSize.Y)
            {
                ;
                for (var x = 0.0f; x < bitmapSize.Width; x += mapTileSize.X)
                {
                    var translation = Matrix3x2.CreateScale(scale) * Matrix3x2.CreateTranslation(x, y);
                    batch.DrawFromSpriteSheet(spriteSheet, translation, sourceRect);
                }
            }
        }
    }
}