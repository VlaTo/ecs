using System;
using ClassLibrary1.Core.Reactive.Collections;

namespace ClassLibrary1.Core
{
    internal class ComponentObserver<TComponent> : ICollectionObserver<IComponent>
        where TComponent : IComponent
    {
        public ComponentObserver(Action<TComponent> onNext)
        {
        }

        public ComponentObserver(Action<TComponent> onNext, Action onComplete)
        {
        }

        public ComponentObserver(Action<TComponent> onNext, Action<Exception> onError, Action onComplete)
        {
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnAdded(IComponent item)
        {
            throw new NotImplementedException();
        }

        public void OnRemoved(IComponent item)
        {
            throw new NotImplementedException();
        }
    }
}