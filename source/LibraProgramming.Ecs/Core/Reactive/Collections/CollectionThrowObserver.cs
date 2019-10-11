using System;
using LibraProgramming.Ecs.Core.Extensions;

namespace LibraProgramming.Ecs.Core.Reactive.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionThrowObserver<T> : ICollectionObserver<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly CollectionThrowObserver<T> Instance;

        private CollectionThrowObserver()
        {
        }

        static CollectionThrowObserver()
        {
            Instance = new CollectionThrowObserver<T>();
        }

        /// <inheritdoc cref="IError.OnError" />
        public void OnError(Exception error)
        {
            error.Throw();
        }

        /// <inheritdoc cref="ICompletable.OnCompleted" />
        public void OnCompleted()
        {
        }

        /// <inheritdoc cref="ICollectionObserver{T}.OnAdded" />
        public void OnAdded(T item)
        {
        }

        /// <inheritdoc cref="ICollectionObserver{T}.OnRemoved" />
        public void OnRemoved(T item)
        {
        }
    }
}