using System;
using LibraProgramming.Ecs.Core.Extensions;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThrowObserver<T> : IObserver<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly ThrowObserver<T> Instance;

        private ThrowObserver()
        {

        }

        static ThrowObserver()
        {
            Instance = new ThrowObserver<T>();
        }

        /// <inheritdoc cref="IObserver{T}.OnCompleted" />
        public void OnCompleted()
        {
        }

        /// <inheritdoc cref="IObserver{T}.OnError" />
        public void OnError(Exception error)
        {
            error.Throw();
        }

        /// <inheritdoc cref="IObserver{T}.OnNext" />
        public void OnNext(T value)
        {
        }
    }
}