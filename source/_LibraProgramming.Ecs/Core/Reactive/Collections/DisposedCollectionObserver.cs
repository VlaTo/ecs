using System;

namespace LibraProgramming.Ecs.Core.Reactive.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisposedCollectionObserver<T> : ICollectionObserver<T>
    {
        public static readonly DisposedCollectionObserver<T> Empty;

        private DisposedCollectionObserver()
        {
        }

        static DisposedCollectionObserver()
        {
            Empty = new DisposedCollectionObserver<T>();
        }

        /// <inheritdoc cref="ICompletable.OnCompleted" />
        public void OnCompleted()
        {
            throw new ObjectDisposedException(String.Empty);
        }

        /// <inheritdoc cref="IError.OnError" />
        public void OnError(Exception error)
        {
            throw new ObjectDisposedException(String.Empty);
        }

        /// <inheritdoc cref="ICollectionObserver{T}.OnAdded" />
        public void OnAdded(T item)
        {
            throw new ObjectDisposedException(String.Empty);
        }

        /// <inheritdoc cref="ICollectionObserver{T}.OnRemoved" />
        public void OnRemoved(T item)
        {
            throw new ObjectDisposedException(String.Empty);
        }
    }
}