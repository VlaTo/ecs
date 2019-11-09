using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EmptyObserver<T> : IObserver<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly EmptyObserver<T> Instance;

        private EmptyObserver()
        {
        }

        static EmptyObserver()
        {
            Instance = new EmptyObserver<T>();
        }

        /// <inheritdoc cref="IObserver{T}.OnCompleted" />
        public void OnCompleted()
        {
        }

        /// <inheritdoc cref="IObserver{T}.OnError" />
        public void OnError(Exception error)
        {
        }

        /// <inheritdoc cref="IObserver{T}.OnNext" />
        public void OnNext(T value)
        {
        }
    }
}