using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class SynchronizedObserver<T> : IObserver<T>
    {
        private readonly IObserver<T> observer;
        private readonly object gate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="gate"></param>
        public SynchronizedObserver(IObserver<T> observer, object gate)
        {
            this.observer = observer;
            this.gate = gate;
        }

        /// <inheritdoc cref="IObserver{T}.OnCompleted" />
        public void OnCompleted()
        {
            lock (gate)
            {
                observer.OnCompleted();
            }
        }

        /// <inheritdoc cref="IObserver{T}.OnError" />
        public void OnError(Exception error)
        {
            lock (gate)
            {
                observer.OnError(error);
            }
        }

        /// <inheritdoc cref="IObserver{T}.OnNext" />
        public void OnNext(T value)
        {
            lock (gate)
            {
                observer.OnNext(value);
            }
        }
    }
}