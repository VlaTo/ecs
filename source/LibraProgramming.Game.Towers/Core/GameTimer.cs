using System;
using LibraProgramming.Ecs.Core.Reactive;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace LibraProgramming.Game.Towers.Core
{
    public class GameTimer : ObservableBase<TimeSpan>, IGameTimer, IDisposable
    {
        private readonly IDisposable disposable;

        public GameTimer(ICanvasAnimatedControl canvasControl)
        {
            disposable = WeakEventListener.AttachEvent<ICanvasAnimatedControl, CanvasAnimatedUpdateEventArgs>(
                handler => canvasControl.Update += handler,
                handler => canvasControl.Update -= handler,
                OnCanvasAnimatedControlUpdate);
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        private void OnCanvasAnimatedControlUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            Next(args.Timing.ElapsedTime);
        }
    }
}