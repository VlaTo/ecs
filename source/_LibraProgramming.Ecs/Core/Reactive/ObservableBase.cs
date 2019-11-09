using System;
using System.Collections.ObjectModel;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableBase<T> : IObservable<T>
    {
        private readonly Collection<IObserver<T>> observers;

        /// <summary>
        /// 
        /// </summary>
        protected ObservableBase()
        {
            observers = new Collection<IObserver<T>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public virtual IDisposable Subscribe(IObserver<T> observer)
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

        /// <summary>
        /// 
        /// </summary>
        public void Release()
        {
            while (0 < observers.Count)
            {
                var last = observers.Count - 1;
                Unsubscribe(observers[last]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        protected void Error(Exception exception)
        {
            foreach (var observer in observers)
            {
                observer.OnError(exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected void Next(T value)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Completed()
        {
            foreach (var observer in observers)
            {
                observer.OnCompleted();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        protected virtual void Unsubscribe(IObserver<T> observer)
        {
            if (observers.Remove(observer))
            {
                observer.OnCompleted();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly ObservableBase<T> owner;
            private readonly IObserver<T> observer;
            private bool disposed;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="observer"></param>
            public Subscription(ObservableBase<T> owner, IObserver<T> observer)
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