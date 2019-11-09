using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Core.Reactive.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public class Entity : EntityBase
    {
        private readonly EntityCollection children;
        private readonly Collection<IComponent> components;
        private readonly IDictionary<Type, Collection<IComponent>> cache;
        private readonly CollectionSubject<IComponent> observers;

        /// <inheritdoc cref="EntityBase.Children" />
        public override IEntityCollection Children => children;

        /// <inheritdoc cref="EntityBase.Components" />
        public override IEnumerable<IComponent> Components => components;

        public override EntityBase Parent
        {
            get => base.Parent;
            set
            {
                base.Parent = value;

                if (null == value)
                {
                    observers.OnCompleted();
                }

                if (null != value)
                {
                    Parent.FixChildEntityKey(this);
                }
            }
        }

        public Entity(string key)
            : base(key)
        {
            children = new EntityCollection(this);
            components = new Collection<IComponent>();
            cache = new Dictionary<Type, Collection<IComponent>>();
            observers = new CollectionSubject<IComponent>();
        }

        /*public Entity(string key, EntityBase prototype)
            : this(key)
        {
            foreach (var component in prototype.Components)
            {
                Add(component.Clone());
            }

            foreach (var child in prototype.Children)
            {
                Children.Add(child.Clone());
            }
        }*/

        /*protected Entity(Entity original)
            : base(original.Key)
        {
            foreach (var component in original.Components)
            {
                Add(component.Clone());
            }

            foreach (var child in original.Children)
            {
                Children.Add(child.Clone());
            }
        }*/

        /// <inheritdoc cref="Subscribe" />
        public override IDisposable Subscribe(ICollectionObserver<IComponent> observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var disposable = observers.Subscribe(observer);

            foreach (var component in components)
            {
                observer.OnAdded(component);
            }

            return disposable;
        }

        /// <inheritdoc cref="EntityBase.Add" />
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

            observers.OnAdded(component);
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

            observers.OnRemoved(component);
            components.Remove(component);

            component.Release();
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

        public override void RemoveAll()
        {
            foreach (var kvp in cache)
            {
                var collection = kvp.Value;

                for (var index = collection.Count - 1; index >= 0; index--)
                {
                    var component = collection[index];

                    try
                    {
                        if (false == collection.Remove(component))
                        {
                            continue;
                        }

                        observers.OnRemoved(component);
                        components.Remove(component);

                        component.Release();
                    }
                    catch (Exception)
                    {
                        ;
                    }
                }
            }
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

        /*protected override void DoFillState(EntityState state)
        {
            var componentStates = new Collection<ComponentState>();
            var entities = new Collection<EntityState>();

            foreach (var component in Components)
            {
                componentStates.Add(component.GetState());
            }

            foreach (var child in Children)
            {
                entities.Add(child.GetState());
            }

            state.EntityPath = null;
            state.Components = componentStates.ToArray();
            state.Children = entities.ToArray();
        }

        protected override void DoApplyState(EntityState state)
        {
            throw new NotImplementedException();
        }*/

        /// <inheritdoc cref="AbstractEntity.SetState" />
        /*public override void SetState(EntityState state)
        {
            if (null == state)
            {
                throw new ArgumentNullException(nameof(state));
            }

            Clear();

            if (false == Key.Equals(state.Key))
            {
                throw new InvalidOperationException();
            }

            foreach (var componentState in state.Components)
            {
                var component = Component.Resolvers.Resolve(componentState.Alias);

                component.SetState(componentState);

                Add(component);
            }
        }*/

        /// <inheritdoc cref="Clone" />
        public override EntityBase Clone()
        {
            var clone = new Entity(Key);
            
            foreach (var component in Components)
            {
                clone.Add(component.Clone());
            }

            foreach (var child in Children)
            {
                clone.Children.Add(child.Clone());
            }

            return clone;
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

        /*private static EntityBase Create(string key, string entityPath)
        {
            if (String.IsNullOrEmpty(entityPath))
            {
                return new Entity(key);
            }

            return new ReferencedEntity(key, entityPath);
        }*/
    }
}