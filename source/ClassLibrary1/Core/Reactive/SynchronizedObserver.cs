using System;

namespace ClassLibrary1.Core.Reactive
{
    internal sealed class SynchronizedObserver<T> : IObserver<T>
    {
        private readonly IObserver<T> observer;
        private readonly object gate;

        public SynchronizedObserver(IObserver<T> observer, object gate)
        {
            this.observer = observer;
            this.gate = gate;
        }

        public void OnCompleted()
        {
            lock (gate)
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            lock (gate)
            {
                observer.OnError(error);
            }
        }

        public void OnNext(T value)
        {
            lock (gate)
            {
                observer.OnNext(value);
            }
        }
    }
}