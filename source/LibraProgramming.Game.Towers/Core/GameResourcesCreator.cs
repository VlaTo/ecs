using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LibraProgramming.Game.Towers.Core
{
    public class GameResourcesCreator : IGameResourcesCreator, IDisposable
    {
        private readonly Collection<IAsyncObserver<ICreateResourcesContext>> observers;
        private readonly IDisposable disposable;
        private bool disposed;

        public GameResourcesCreator(ICanvasAnimatedControl canvasControl)
        {
            observers = new Collection<IAsyncObserver<ICreateResourcesContext>>();
            disposable = WeakEventListener.AttachEvent<CanvasAnimatedControl, CanvasCreateResourcesEventArgs>(
                handler => canvasControl.CreateResources += handler,
                handler => canvasControl.CreateResources -= handler,
                OnCanvasAnimatedControlCreateResources);
        }

        public IDisposable Subscribe(IAsyncObserver<ICreateResourcesContext> observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            if (false == observers.Contains(observer))
            {
                observers.Add(observer);
            }

            return new Subscription(this, observer);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void ReleaseObservers()
        {
            while (0 < observers.Count)
            {
                var last = observers.Count - 1;
                Unsubscribe(observers[last]);
            }
        }

        private void OnCanvasAnimatedControlCreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(DoCreateResourcesAsync(sender).AsAsyncAction());
        }

        private async Task DoCreateResourcesAsync(CanvasAnimatedControl control)
        {
            using (var context = new CanvasCreateResourcesContext(control, control.Size))
            {
                foreach (var observer in observers)
                {
                    await observer.OnNextAsync(context);
                }
            }
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    disposable.Dispose();
                    ReleaseObservers();
                }
            }
            finally
            {
                disposed = true;
            }
        }

        private void Unsubscribe(IAsyncObserver<ICreateResourcesContext> observer)
        {
            if (observers.Remove(observer))
            {
                observer.OnCompleted();
            }
        }

        private sealed class CanvasCreateResourcesContext : ICreateResourcesContext, IDisposable
        {
            public ICanvasResourceCreatorWithDpi Creator
            {
                get;
            }

            public Size CanvasSize
            {
                get;
            }
            
            public CanvasCreateResourcesContext(CanvasAnimatedControl control, Size size)
            {
                Creator = control;
                CanvasSize = size;
            }

            public void Dispose()
            {
                ;
            }
        }

        private class Subscription : IDisposable
        {
            private readonly GameResourcesCreator owner;
            private readonly IAsyncObserver<ICreateResourcesContext> observer;
            private bool disposed;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="observer"></param>
            public Subscription(GameResourcesCreator owner, IAsyncObserver<ICreateResourcesContext> observer)
            {
                this.owner = owner;
                this.observer = observer;
            }

            /// <inheritdoc cref="IDisposable.Dispose" />
            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool dispose)
            {
                if (disposed)
                {
                    return;
                }

                try
                {
                    if (dispose)
                    {
                        owner.Unsubscribe(observer);
                    }
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}