using System;
using System.Collections.Immutable;

namespace ClassLibrary1.Core.Reactive
{
    public class ListObserver<T> : IObserver<T>
    {
        private readonly ImmutableList<IObserver<T>> observers;

        public ListObserver(ImmutableList<IObserver<T>> observers)
        {
            this.observers = observers;
        }

        public void OnCompleted()
        {
            observers.ForEach(observer => observer.OnCompleted());
        }

        public void OnError(Exception error)
        {
            observers.ForEach(observer => observer.OnError(error));
        }

        public void OnNext(T value)
        {
            observers.ForEach(observer => observer.OnNext(value));
        }

        internal IObserver<T> Add(IObserver<T> observer)
        {
            return new ListObserver<T>(observers.Add(observer));
        }

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