using System;

namespace ClassLibrary1.Core.Reactive
{
    public class DisposedObserver<T> : IObserver<T>
    {
        public static readonly DisposedObserver<T> Instance;

        private DisposedObserver()
        {
        }

        static DisposedObserver()
        {
            Instance = new DisposedObserver<T>();
        }

        public void OnCompleted()
        {
            throw new ObjectDisposedException(String.Empty);
        }

        public void OnError(Exception error)
        {
            throw new ObjectDisposedException(String.Empty);
        }

        public void OnNext(T value)
        {
            throw new ObjectDisposedException(String.Empty);
        }
    }
}