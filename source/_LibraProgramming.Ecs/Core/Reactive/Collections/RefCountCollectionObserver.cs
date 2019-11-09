using System;

namespace LibraProgramming.Ecs.Core.Reactive.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RefCountCollectionObserver<T>
    {
        private readonly object gate;
        private ICollectionObserver<T> observer;
        private int count;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        public RefCountCollectionObserver(ICollectionObserver<T> observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            this.observer = observer;

            gate = new object();
            count = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollectionObserver<T> GetObserver()
        {
            lock (gate)
            {
                if (null == observer)
                {
                    return DisposedCollectionObserver<T>.Empty;
                }

                count++;

                return new InnerCollectionObserver(this);
            }
        }

        private void Release()
        {
            var promise = default(ICollectionObserver<T>);

            lock (gate)
            {
                if (null != observer)
                {
                    if (0 == --count)
                    {
                        promise = observer;
                        observer = null;
                    }
                }
            }

            if (null != promise)
            {
                promise.OnCompleted();
            }
        }

        private void OnError(Exception error)
        {
            observer.OnError(error);
        }

        private void OnAdded(T item)
        {
            observer.OnAdded(item);
        }

        private void OnRemoved(T item)
        {
            observer.OnRemoved(item);
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class InnerCollectionObserver : ICollectionObserver<T>
        {
            private readonly object gate;
            private RefCountCollectionObserver<T> owner;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="owner"></param>
            public InnerCollectionObserver(RefCountCollectionObserver<T> owner)
            {
                this.owner = owner;
                gate = new object();
            }

            /// <inheritdoc cref="IError.OnError" />
            public void OnError(Exception error)
            {
                owner.OnError(error);
            }

            /// <inheritdoc cref="ICompletable.OnCompleted" />
            public void OnCompleted()
            {
                RefCountCollectionObserver<T> temp;

                lock (gate)
                {
                    temp = owner;
                    owner = null;
                }

                if (null != temp)
                {
                    temp.Release();
                }
            }

            /// <inheritdoc cref="ICollectionObserver{T}.OnAdded" />
            public void OnAdded(T item)
            {
                owner.OnAdded(item);
            }

            /// <inheritdoc cref="ICollectionObserver{T}.OnRemoved" />
            public void OnRemoved(T item)
            {
                owner.OnRemoved(item);
            }
        }
    }
}