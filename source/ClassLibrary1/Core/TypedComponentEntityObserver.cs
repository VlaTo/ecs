using System;
using System.Collections.Generic;

namespace ClassLibrary1.Core
{
    internal sealed class TypedComponentEntityObserver<TComponent> : IEntityObserver
        where TComponent : IComponent
    {
        private readonly Type componentType;
        private readonly IEntityObserver<TComponent> observer;
        private readonly IList<IComponent> components;

        public TypedComponentEntityObserver(IEntityObserver<TComponent> observer)
        {
            componentType = typeof(TComponent);
            this.observer = observer;
            components = new List<IComponent>();
        }

        void IEntityObserver.OnAdded(IComponent component)
        {
            if (componentType != component.GetType())
            {
                return;
            }

            components.Add(component);

            observer.OnAdded((TComponent) component);
        }

        void ICompletable.OnCompleted()
        {
            observer.OnCompleted();
        }

        void IError.OnError(Exception error)
        {
            observer.OnError(error);
        }

        void IEntityObserver.OnRemoved(IComponent component)
        {
            if (components.Remove(component))
            {
                observer.OnRemoved((TComponent) component);
            }
        }
    }
}