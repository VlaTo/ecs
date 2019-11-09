using System;
using System.Collections.Immutable;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListObserver<T> : IObserver<T>
    {
        private readonly ImmutableList<IObserver<T>> observers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observers"></param>
        public ListObserver(ImmutableList<IObserver<T>> observers)
        {
            this.observers = observers;
        }

        /// <inheritdoc cref="IObserver{T}.OnCompleted" />
        public void OnCompleted()
        {
            observers.ForEach(observer => observer.OnCompleted());
        }

        /// <inheritdoc cref="IObserver{T}.OnError" />
        public void OnError(Exception error)
        {
            observers.ForEach(observer => observer.OnError(error));
        }

        /// <inheritdoc cref="IObserver{T}.OnNext" />
        public void OnNext(T value)
        {
            observers.ForEach(observer => observer.OnNext(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        internal IObserver<T> Add(IObserver<T> observer)
        {
            return new ListObserver<T>(observers.Add(observer));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        internal IObserver<T> Remove(IObserver<T> observer)
        {
            var index = observers.FindIndex(current => current == observer);

            if (0 > index)
            {
                return this;
            }

            if (2 == observers.Count)
            {
                return observers[1 - index];
            }

            return new ListObserver<T>(observers.Remove(observer));
        }
    }
}