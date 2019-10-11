using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DisposedObserver<T> : IObserver<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DisposedObserver<T> Instance;

        private DisposedObserver()
        {
        }

        static DisposedObserver()
        {
            Instance = new DisposedObserver<T>();
        }

        /// <inheritdoc cref="IObserver{T}.OnCompleted" />
        public void OnCompleted()
        {
            throw new ObjectDisposedException(String.Empty);
        }

        /// <inheritdoc cref="IObserver{T}.OnError" />
        public void OnError(Exception error)
        {
            throw new ObjectDisposedException(String.Empty);
        }

        /// <inheritdoc cref="IObserver{T}.OnNext" />
        public void OnNext(T value)
        {
            throw new ObjectDisposedException(String.Empty);
        }
    }
}