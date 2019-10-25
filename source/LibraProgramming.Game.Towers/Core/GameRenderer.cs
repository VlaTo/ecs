using System;
using LibraProgramming.Ecs.Core.Reactive;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace LibraProgramming.Game.Towers.Core
{
    public sealed class GameRenderer : ObservableBase<IGameRenderContext>, IGameRenderer, IDisposable
    {
        private readonly IDisposable disposable;

        public GameRenderer(ICanvasAnimatedControl canvasControl)
        {
            disposable = WeakEventListener.AttachEvent<ICanvasAnimatedControl, CanvasAnimatedDrawEventArgs>(
                handler => canvasControl.Draw += handler,
                handler => canvasControl.Draw -= handler,
                OnCanvasAnimatedControlDraw);
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        private void OnCanvasAnimatedControlDraw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var renderContext = new GameRenderContext(args.DrawingSession);
            Next(renderContext);
        }
    }
}