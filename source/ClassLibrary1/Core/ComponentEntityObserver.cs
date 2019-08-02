using System;
using System.Collections.Generic;

namespace ClassLibrary1.Core
{
    internal sealed class ComponentEntityObserver : ICollectionObserver<IComponent>
    {
        private readonly ICollectionObserver<IComponent> observer;
        private readonly IList<IComponent> components;

        public ComponentEntityObserver(ICollectionObserver<IComponent> observer)
        {
            this.observer = observer;
            components = new List<IComponent>();
        }

        void ICollectionObserver<IComponent>.OnAdded(IComponent component)
        {
            components.Add(component);
            observer.OnAdded(component);
        }

        void ICompletable.OnCompleted()
        {
            observer.OnCompleted();
        }

        void IError.OnError(Exception error)
        {
            observer.OnError(error);
        }

        void ICollectionObserver<IComponent>.OnRemoved(IComponent component)
        {
            if (components.Remove(component))
            {
                observer.OnRemoved(component);
            }
        }
    }
}