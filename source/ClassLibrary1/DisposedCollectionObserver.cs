using System;

namespace ClassLibrary1
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