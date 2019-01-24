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
    public sealed class EntityImplementation : Entity
    {
        private readonly EntityCollection children;
        private readonly ICollection<IComponent> components;
        private readonly IDictionary<Type, Collection<IComponent>> cache;
        private readonly ICollection<IEntityObserver> observers;

        /// <inheritdoc cref="Entity.Children" />
        public override IEntityCollection Children => children;

        /// <inheritdoc cref="Entity.Components" />
        public override IEnumerable<IComponent> Components => components;

        internal EntityImplementation(string key)
            : base(key)
        {
            children = new EntityCollection(this);
            components = new Collection<IComponent>();
            cache = new Dictionary<Type, Collection<IComponent>>();
            observers = new List<IEntityObserver>();
        }

        /// <inheritdoc cref="Subscribe" />
        public override IDisposable Subscribe(IEntityObserver observer)
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

        /// <inheritdoc cref="Entity.Add" />
        public override void Add(IComponent component)
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

        /// <inheritdoc cref="Remove" />
        public override void Remove(IComponent component)
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

        /// <inheritdoc cref="Get{TComponent}" />
        public override TComponent Get<TComponent>()
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

            return (TComponent)collection[0];
        }

        /// <inheritdoc cref="GetAll{TComponent}" />
        public override IReadOnlyCollection<TComponent> GetAll<TComponent>()
        {
            var key = typeof(TComponent);
            var result = new List<TComponent>();

            if (cache.TryGetValue(key, out var collection))
            {
                result.AddRange(collection.OfType<TComponent>());
            }

            return new ReadOnlyCollection<TComponent>(result);
        }

        /// <inheritdoc cref="Has" />
        public override bool Has(IComponent component)
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

        /// <inheritdoc cref="Has{TComponent}" />
        public override bool Has<TComponent>()
        {
            var key = typeof(TComponent);

            if (false == cache.TryGetValue(key, out var collection))
            {
                return false;
            }

            return collection.Any();
        }

        /// <inheritdoc cref="Find{TComponent}" />
        public override IEnumerable<TComponent> Find<TComponent>()
        {
            var collection = new List<TComponent>();
            Entity current = this;

            while (null != current)
            {
                collection.AddRange(current.GetAll<TComponent>());

                current = current.Parent;
            }

            return collection;
        }

        /// <inheritdoc cref="GetState" />
        public override EntityState GetState()
        {
            return new EntityState
            {

            };
        }

        protected override void Dispose(bool dispose)
        {
            if (Disposed)
            {
                return;
            }

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

        /// <summary>
        /// 
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly EntityImplementation entity;
            private readonly IEntityObserver observer;
            private bool disposed;

            public Subscription(EntityImplementation entity, IEntityObserver observer)
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