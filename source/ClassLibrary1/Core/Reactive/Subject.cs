using System;
using System.Collections.Immutable;

namespace ClassLibrary1.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Subject<T> : ISubject<T>, IDisposable
    {
        private readonly object gate;
        private IObserver<T> current;
        private bool stopped;
        private bool disposed;
        private Exception exception;

        public bool HasObservers => false == (current is EmptyObserver<T>) && false == stopped && false == disposed;

        public bool IsRequiredSubscribeOnCurrentThread => false;

        public Subject()
        {
            gate = new object();
            current = EmptyObserver<T>.Instance;
        }

        /// <inheritdoc cref="ObservableBase{T}.Subscribe" />
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            Exception ex;

            lock (gate)
            {
                ThrowIfDisposed();

                if (false == stopped)
                {
                    if (current is ListObserver<T> listObserver)
                    {
                        current = listObserver.Add(observer);
                    }
                    else if (current is EmptyObserver<T>)
                    {
                        current = observer;
                    }
                    else
                    {
                        var observers = ImmutableList<IObserver<T>>.Empty.AddRange(new[] {current, observer});
                        current = new ListObserver<T>(observers);
                    }

                    return new Subscription(this, observer);
                }

                ex = exception;
            }

            if (null != ex)
            {
                observer.OnError(ex);
            }
            else
            {
                observer.OnCompleted();
            }

            return Disposable.Empty;
        }

        void IObserver<T>.OnCompleted()
        {
            IObserver<T> observer;

            lock (gate)
            {
                ThrowIfDisposed();

                if (stopped)
                {
                    return;
                }

                observer = current;
                current = EmptyObserver<T>.Instance;

                stopped = true;
            }

            observer.OnCompleted();
        }

        void IObserver<T>.OnError(Exception error)
        {
            if (null == error)
            {
                throw new ArgumentNullException(nameof(error));
            }

            IObserver<T> observer;

            lock (gate)
            {
                ThrowIfDisposed();

                if (stopped)
                {
                    return;
                }

                observer = current;
                current = EmptyObserver<T>.Instance;

                stopped = true;
                exception = error;
            }

            observer.OnError(error);
        }

        void IObserver<T>.OnNext(T value)
        {
            current.OnNext(value);
        }

        void IDisposable.Dispose()
        {
            lock (gate)
            {
                disposed = true;
                current = DisposedObserver<T>.Instance;
            }
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(String.Empty);
            }
        }

        private void RemoveObserver(IObserver<T> observer)
        {
            lock (gate)
            {
                if (current is ListObserver<T> listObserver)
                {
                    current = listObserver.Remove(observer);
                }
                else
                {
                    current = EmptyObserver<T>.Instance;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly object gate;
            private Subject<T> parent;
            private IObserver<T> observer;

            public Subscription(Subject<T> parent, IObserver<T> observer)
            {
                this.parent = parent;
                this.observer = observer;
                gate = new object();
            }

            public void Dispose()
            {
                lock (gate)
                {
                    if (null != parent)
                    {
                        parent.RemoveObserver(observer);

                        observer = null;
                        parent = null;
                    }
                }
            }
        }
    }
}