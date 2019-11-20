using LibraProgramming.Ecs.Core.Reactive;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;

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
                (sender, args) =>
                {
                    var renderContext = new GameRenderContext(args.DrawingSession, sender.Size);
                    Next(renderContext);
                }
            );
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}