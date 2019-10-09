using System;
using ClassLibrary1.Core.Extensions;

namespace ClassLibrary1.Core.Reactive.Collections
{
    public class CollectionThrowObserver<T> : ICollectionObserver<T>
    {
        public static readonly CollectionThrowObserver<T> Instance;

        private CollectionThrowObserver()
        {
        }

        static CollectionThrowObserver()
        {
            Instance = new CollectionThrowObserver<T>();
        }

        public void OnError(Exception error)
        {
            error.Throw();
        }

        public void OnCompleted()
        {
        }

        public void OnAdded(T item)
        {
        }

        public void OnRemoved(T item)
        {
        }
    }
}