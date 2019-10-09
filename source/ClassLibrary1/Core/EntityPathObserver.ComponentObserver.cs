using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ClassLibrary1.Core.Reactive.Collections;

namespace ClassLibrary1.Core
{
    /*internal partial class LiveEntityPathObserver<TComponent>
    {
        /// <summary>
        /// 
        /// </summary>
        private class ComponentObserver : IComponentEnumerable<TComponent>, ICollectionObserver<IComponent>
        {
            private readonly LiveEntityPathObserver<TComponent> observer;
            private readonly List<TComponent> components;
            private bool disposed;

            public ComponentObserver(LiveEntityPathObserver<TComponent> observer)
            {
                components = new List<TComponent>();
                this.observer = observer;
            }

            public IEnumerator<TComponent> GetEnumerator()
            {
                return components.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void Subscribe(EntityBase entity)
            {
                if (null == entity)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                var disposable = entity.Subscribe<IComponent, EntityBase>();
            }

            void IError.OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            void ICompletable.OnCompleted()
            {
                throw new NotImplementedException();
            }

            void ICollectionObserver<IComponent>.OnAdded(IComponent item)
            {
                if (item is TComponent component)
                {
                    components.Add(component);
                }
            }

            void ICollectionObserver<IComponent>.OnRemoved(IComponent item)
            {
                if (item is TComponent component)
                {
                    if (components.Remove(component))
                    {

                    }
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool dispose)
            {
                if (disposed)
                {
                    return;
                }

                try
                {
                    if (dispose)
                    {
                        ;
                    }
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }*/
}