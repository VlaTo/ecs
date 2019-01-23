using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ClassLibrary1.Core;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public class Entity : IObservableEntity, IDisposable
    {
        private readonly EntityCollection children;
        private readonly ICollection<IEntityObserver> observers;
        private readonly ICollection<IComponent> components;
        private readonly IDictionary<Type, Collection<IComponent>> cache;
        private Entity parent;
        private bool disposed;

        public IEntityCollection Children => children;

        public IEnumerable<IComponent> Components => components;

        public Entity Parent
        {
            get => parent;
            set
            {
                if (ReferenceEquals(parent, value))
                {
                    return;
                }

                if (null != parent)
                {
                    parent.Children.Remove(this);
                }

                parent = value;

                if (null != parent)
                {
                    parent.Children.Add(this);
                }
            }
        }

        public Entity Root
        {
            get
            {
                var current = this;
                var next = Parent;

                while (null != next)
                {
                    current = next;
                    next = current.Parent;
                }

                return current;
            }
        }

        public Entity()
        {
            children = new EntityCollection(this);
            observers = new List<IEntityObserver>();
            components = new Collection<IComponent>();
            cache = new Dictionary<Type, Collection<IComponent>>();
        }

        public IDisposable Subscribe(IEntityObserver observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            if (false == observers.Contains(observer))
            {
                observers.Add(observer);
            }

            foreach (var component in Components)
            {
                DoComponentAdded(component);
            }

            return new Subscription(this, observer);
        }

        public void AddComponent(IComponent component)
        {
            if (null == component)
            {
                throw new ArgumentNullException(nameof(component));
            }

            var key = component.GetType();

            if (false == cache.TryGetValue(key, out var collection))
            {
                collection = new Collection<IComponent>();
                cache.Add(key, collection);
            }

            if (collection.Contains(component))
            {
                return;
            }

            components.Add(component);
            collection.Add(component);

            component.Attach(this);

            DoComponentAdded(component);
        }

        public TComponent AddComponent<TComponent>(Action<TComponent> initializer = null)
            where TComponent : IComponent, new()
        {
            var component = new TComponent();

            initializer?.Invoke(component);

            AddComponent(component);

            return component;
        }

        public void RemoveComponent(IComponent component)
        {
            if (null == component)
            {
                throw new ArgumentNullException(nameof(component));
            }

            var key = component.GetType();

            if (false == cache.TryGetValue(key, out var collection))
            {
                return;
            }

            if (false == collection.Remove(component))
            {
                return;
            }

            components.Remove(component);
            component.Release();

            DoComponentRemoved(component);
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : class, IComponent
        {
            var key = typeof(TComponent);

            if (false == cache.TryGetValue(key, out var collection))
            {
                return null;
            }

            if (1 < collection.Count)
            {
                throw new InvalidOperationException();
            }

            return (TComponent) collection[0];
        }

        public IReadOnlyCollection<TComponent> GetComponents<TComponent>()
            where TComponent : class, IComponent
        {
            var key = typeof(TComponent);
            var result = new List<TComponent>();

            if (cache.TryGetValue(key, out var collection))
            {
                result.AddRange(collection.OfType<TComponent>());
            } 

            return new ReadOnlyCollection<TComponent>(result);
        }

        public bool HasComponent(IComponent component)
        {
            if (null == component)
            {
                throw new ArgumentNullException(nameof(component));
            }

            var key = component.GetType();

            if (false == cache.TryGetValue(key, out var collection))
            {
                return false;
            }

            return collection.Contains(component);
        }

        public bool HasComponents<TComponent>()
            where TComponent : IComponent
        {
            var key = typeof(TComponent);

            if (false == cache.TryGetValue(key, out var collection))
            {
                return false;
            }

            return collection.Any();
        }

        public IEnumerable<TComponent> FindComponents<TComponent>()
            where TComponent : class, IComponent
        {
            var collection = new List<TComponent>();
            var current = this;

            while (null != current)
            {
                collection.AddRange(current.GetComponents<TComponent>());

                current = current.Parent;
            }

            return collection;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Unsubscribe(IEntityObserver observer)
        {
            if (false == observers.Remove(observer))
            {
                return;
            }

            observer.OnCompleted();
        }

        private void DoComponentAdded(IComponent component)
        {
            foreach (var observer in observers)
            {
                observer.OnAdded(component);
            }
        }

        private void DoComponentRemoved(IComponent component)
        {
            foreach (var observer in observers)
            {
                observer.OnRemoved(component);
            }
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
                    var handlers = observers.ToArray();

                    observers.Clear();
                    
                    foreach (var observer in handlers)
                    {
                        observer.OnCompleted();
                    }
                }
            }
            finally
            {
                disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly Entity entity;
            private readonly IEntityObserver observer;
            private bool disposed;

            public Subscription(Entity entity, IEntityObserver observer)
            {
                this.entity = entity;
                this.observer = observer;
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
                        entity.Unsubscribe(observer);
                    }
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}