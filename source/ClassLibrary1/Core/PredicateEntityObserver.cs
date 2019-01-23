using System;
using System.Collections.Generic;

namespace ClassLibrary1.Core
{
    internal sealed class PredicateEntityObserver : IEntityObserver
    {
        private readonly Predicate<IComponent> predicate;
        private readonly IEntityObserver observer;
        private readonly IList<IComponent> components;

        public PredicateEntityObserver(Predicate<IComponent> predicate, IEntityObserver observer)
        {
            this.predicate = predicate;
            this.observer = observer;

            components = new List<IComponent>();
        }

        public void OnAdded(IComponent component)
        {
            if (false == predicate.Invoke(component))
            {
                return;
            }

            components.Add(component);
            observer.OnAdded(component);
        }

        public void OnCompleted()
        {
            foreach (var component in components)
            {
                components.Remove(component);
            }

            observer.OnCompleted();
        }

        public void OnError(Exception error)
        {
            foreach (var component in components)
            {
                components.Remove(component);
            }

            observer.OnError(error);
        }

        public void OnRemoved(IComponent component)
        {
            if (components.Remove(component))
            {
                observer.OnRemoved(component);
            }
        }
    }
}