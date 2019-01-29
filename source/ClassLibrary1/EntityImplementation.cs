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
        private string keyOverride;

        /// <inheritdoc cref="Entity.Key" />
        public override string Key => keyOverride ?? base.Key;

        /// <inheritdoc cref="Entity.Children" />
        public override IEntityCollection Children => children;

        /// <inheritdoc cref="Entity.Components" />
        public override IEnumerable<IComponent> Components => components;

        public EntityImplementation(string key)
            : base(key)
        {
            children = new EntityCollection(this);
            components = new Collection<IComponent>();
            cache = new Dictionary<Type, Collection<IComponent>>();
            observers = new List<IEntityObserver>();
        }

        public EntityImplementation(EntityImplementation instance)
            : this(instance.Key)
        {
            foreach (var component in instance.Components)
            {
                Add(component.Clone());
            }

            foreach (var child in instance.Children)
            {
                Children.Add(child.Clone());
            }
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
                throw new EntityException();
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

        /// <inheritdoc cref="Find" />
        public override IEnumerable<Entity> Find(EntityPathString path)
        {
            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            throw new NotImplementedException();
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

        /// <inheritdoc cref="GetState" />
        public override EntityState GetState()
        {
            var states = new Collection<ComponentState>();
            var entities = new Collection<EntityState>();

            foreach (var component in Components)
            {
                states.Add(component.GetState());
            }

            foreach (var child in Children)
            {
                entities.Add(child.GetState());
            }

            return new EntityState
            {
                Key = Key,
                EntityPath = null,
                Components = states.ToArray(),
                Children = entities.ToArray()
            };
        }

        /// <inheritdoc cref="Entity.SetState" />
        public override void SetState(EntityState state)
        {
            if (null == state)
            {
                throw new ArgumentNullException(nameof(state));
            }

            Clear();

            if (false == Key.Equals(state.Key))
            {
                keyOverride = state.Key;
            }

            foreach (var componentState in state.Components)
            {
                var component = Component.Resolvers.Resolve(componentState.Alias);

                component.SetState(componentState);

                Add(component);
            }

            foreach (var entityState in state.Children)
            {
                var entity = Create(state.Key, state.EntityPath);

                entity.SetState(entityState);

                Children.Add(entity);
            }
        }

        /// <inheritdoc cref="Clone" />
        public override Entity Clone()
        {
            return new EntityImplementation(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (var component in Components)
            {
                Remove(component);
            }

            Children.Clear();
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

        private static Entity Create(string key, string entityPath)
        {
            if (String.IsNullOrEmpty(entityPath))
            {
                return new EntityImplementation(key);
            }

            return new EntityReference(key, entityPath);
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