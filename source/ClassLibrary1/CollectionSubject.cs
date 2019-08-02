using System;
using System.Collections.Immutable;
using ClassLibrary1.Core;

namespace ClassLibrary1
{
    public sealed class CollectionSubject<T> : IObservableCollection<T>, ICollectionObserver<T>, IDisposable
    {
        private readonly object gate;
        private ICollectionObserver<T> current;
        private bool stopped;
        private bool disposed;
        private Exception exception;

        public CollectionSubject()
        {
            gate = new object();
            current = EmptyObserver.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(ICollectionObserver<T> observer)
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
                    if (current is ListObserver listObserver)
                    {
                        current = listObserver.Add(observer);
                    }
                    else if (current is EmptyObserver)
                    {
                        current = observer;
                    }
                    else
                    {
                        var observers = ImmutableList<ICollectionObserver<T>>.Empty.AddRange(new[] {current, observer});
                        current = new ListObserver(observers);
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

        public void Dispose()
        {
            lock (gate)
            {
                disposed = true;
                current = DisposedObserver.Instance;
            }
        }

        public void OnError(Exception error)
        {
            if (null == error)
            {
                throw new ArgumentNullException(nameof(error));
            }

            ICollectionObserver<T> observer;

            lock (gate)
            {
                ThrowIfDisposed();

                if (stopped)
                {
                    return;
                }

                observer = current;
                current = EmptyObserver.Instance;

                stopped = true;
                exception = error;
            }

            observer.OnError(error);
        }

        public void OnCompleted()
        {
            ICollectionObserver<T> observer;

            lock (gate)
            {
                ThrowIfDisposed();

                if (stopped)
                {
                    return;
                }

                observer = current;
                current = EmptyObserver.Instance;

                stopped = true;
            }

            observer.OnCompleted();
        }

        public void OnAdded(T item)
        {
            current.OnAdded(item);
        }

        public void OnRemoved(T item)
        {
            current.OnRemoved(item);
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(String.Empty);
            }
        }

        private void RemoveObserver(ICollectionObserver<T> observer)
        {
            lock (gate)
            {
                if (current is ListObserver listObserver)
                {
                    current = listObserver.Remove(observer);
                }
                else
                {
                    current = EmptyObserver.Instance;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly object gate;
            private CollectionSubject<T> parent;
            private ICollectionObserver<T> observer;

            public Subscription(CollectionSubject<T> parent, ICollectionObserver<T> observer)
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

                        observer.OnCompleted();

                        observer = null;
                        parent = null;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class EmptyObserver : ICollectionObserver<T>
        {
            public static readonly EmptyObserver Instance;

            private EmptyObserver()
            {
            }

            static EmptyObserver()
            {
                Instance = new EmptyObserver();
            }

            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnAdded(T item)
            {
            }

            public void OnRemoved(T item)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class ListObserver : ICollectionObserver<T>
        {
            private readonly ImmutableList<ICollectionObserver<T>> observers;

            public ListObserver(ImmutableList<ICollectionObserver<T>> observers)
            {
                this.observers = observers;
            }

            public void OnCompleted()
            {
                observers.ForEach(observer => observer.OnCompleted());
            }

            public void OnError(Exception error)
            {
                observers.ForEach(observer => observer.OnError(error));
            }

            public void OnAdded(T item)
            {
                observers.ForEach(observer => observer.OnAdded(item));
            }

            public void OnRemoved(T item)
            {
                observers.ForEach(observer => observer.OnRemoved(item));
            }

            internal ICollectionObserver<T> Add(ICollectionObserver<T> observer)
            {
                return new ListObserver(observers.Add(observer));
            }

            internal ICollectionObserver<T> Remove(ICollectionObserver<T> observer)
            {
                var index = observers.FindIndex(current => current == observer);

                if (0 > index)
                {
                    return this;
                }

                if (2 == observers.Count)
                {
                    return observers[1 - index];
                }

                return new ListObserver(observers.Remove(observer));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class DisposedObserver : ICollectionObserver<T>
        {
            public static readonly DisposedObserver Instance;

            private DisposedObserver()
            {
            }

            static DisposedObserver()
            {
                Instance = new DisposedObserver();
            }

            public void OnCompleted()
            {
                throw new ObjectDisposedException(String.Empty);
            }

            public void OnError(Exception error)
            {
                throw new ObjectDisposedException(String.Empty);
            }

            public void OnAdded(T item)
            {
                throw new ObjectDisposedException(String.Empty);
            }

            public void OnRemoved(T item)
            {
                throw new ObjectDisposedException(String.Empty);
            }
        }
    }
}