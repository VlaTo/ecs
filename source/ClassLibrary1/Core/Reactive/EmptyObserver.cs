using System;

namespace ClassLibrary1.Core.Reactive
{
    public sealed class EmptyObserver<T> : IObserver<T>
    {
        public static readonly EmptyObserver<T> Instance;

        private EmptyObserver()
        {
        }

        static EmptyObserver()
        {
            Instance = new EmptyObserver<T>();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(T value)
        {
        }
    }
}