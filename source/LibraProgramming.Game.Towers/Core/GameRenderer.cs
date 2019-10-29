using LibraProgramming.Ecs.Core.Reactive;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace LibraProgramming.Game.Towers.Core
{
    public sealed class GameRenderer : ObservableBase<IGameRenderContext>, IGameRenderer, IDisposable
    {
        private readonly IDisposable disposable;
        private CanvasBitmap enemyBitmap;

        public GameRenderer(ICanvasAnimatedControl canvasControl)
        {
            disposable = new CompositeDisposable(
                WeakEventListener.AttachEvent<ICanvasAnimatedControl, CanvasAnimatedDrawEventArgs>(
                    handler => canvasControl.Draw += handler,
                    handler => canvasControl.Draw -= handler,
                    OnCanvasAnimatedControlDraw),
                WeakEventListener.AttachEvent<CanvasAnimatedControl, CanvasCreateResourcesEventArgs>(
                    handler => canvasControl.CreateResources += handler,
                    handler => canvasControl.CreateResources -= handler,
                    OnCanvasAnimatedControlCreateResources)
            );
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        private void OnCanvasAnimatedControlDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var renderList = new List<Vector2>(40);
            var renderContext = new GameRenderContext(renderList);

            Next(renderContext);

            if (0 < renderList.Count)
            {
                using (var batch = args.DrawingSession.CreateSpriteBatch(CanvasSpriteSortMode.Bitmap))
                {
                    foreach (var position in renderList)
                    {
                        batch.Draw(enemyBitmap, position);
                    }
                }
            }
        }

        private void OnCanvasAnimatedControlCreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(DoCreateResourcesAsync(sender).AsAsyncAction());
        }

        private Task DoCreateResourcesAsync(ICanvasResourceCreatorWithDpi resourceCreator)
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
                enemyBitmap = CanvasBitmap.CreateFromBytes(
                    resourceCreator,
                    renderTarget.GetPixelBytes(),
                    (int) size.Width,
                    (int) size.Height,
                    renderTarget.Format
                );
            }

            return Task.CompletedTask;
        }
    }
}