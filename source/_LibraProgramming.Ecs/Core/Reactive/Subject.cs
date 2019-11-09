using System;
using System.Collections.Immutable;

namespace LibraProgramming.Ecs.Core.Reactive
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

        /// <summary>
        /// 
        /// </summary>
        public bool HasObservers => false == (current is EmptyObserver<T>) && false == stopped && false == disposed;

        /// <summary>
        /// 
        /// </summary>
        public bool IsRequiredSubscribeOnCurrentThread => false;

        /// <summary>
        /// 
        /// </summary>
        public Subject()
        {
            gate = new object();
            current = EmptyObserver<T>.Instance;
        }

        /// <inheritdoc cref="IObservable{T}.Subscribe" />
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

        /// <inheritdoc cref="IObserver{T}.OnCompleted" />
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

        /// <inheritdoc cref="IObserver{T}.OnError" />
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

        /// <inheritdoc cref="IObserver{T}.OnNext" />
        void IObserver<T>.OnNext(T value)
        {
            current.OnNext(value);
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
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

            /// <summary>
            /// 
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="observer"></param>
            public Subscription(Subject<T> parent, IObserver<T> observer)
            {
                this.parent = parent;
                this.observer = observer;
                gate = new object();
            }

            /// <inheritdoc cref="IDisposable.Dispose" />
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