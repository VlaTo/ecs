using System;
using System.Collections.Immutable;

namespace ClassLibrary1.Core
{
    public sealed class ListEntityObserver : IEntityObserver
    {
        private readonly ImmutableList<IEntityObserver> observers;

        public ListEntityObserver(ImmutableList<IEntityObserver> observers)
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

        public void OnAdded(IComponent component)
        {
            observers.ForEach(observer => observer.OnAdded(component));
        }

        public void OnRemoved(IComponent component)
        {
            observers.ForEach(observer => observer.OnRemoved(component));
        }

        internal IEntityObserver Add(IEntityObserver observer)
        {
            return new ListEntityObserver(observers.Add(observer));
        }

        internal IEntityObserver Remove(IEntityObserver observer)
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

            return new ListEntityObserver(observers.Remove(observer));
        }
    }
}