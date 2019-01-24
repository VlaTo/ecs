using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Entity : IObservableEntity, IStateProvider<EntityState>, IDisposable
    {
        internal const char Separator = '/';

        private Entity parent;

        protected bool Disposed
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract IEntityCollection Children
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract IEnumerable<IComponent> Components
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Entity Parent
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

        /// <summary>
        /// 
        /// </summary>
        public virtual EntityPathString Path
        {
            get
            {
                var queue = new Stack<string>();
                var current = this;

                while (null != current)
                {
                    queue.Push(current.Key);
                    current = current.Parent;
                }

                queue.Push(String.Empty);

                return new EntityPathString(String.Join(Separator, queue));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Entity Root
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

        protected Entity()
        {
        }

        protected Entity(string key)
            : this()
        {
            Key = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public abstract IDisposable Subscribe(IEntityObserver observer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        public abstract void Add(IComponent component);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        public abstract void Remove(IComponent component);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public virtual TComponent Add<TComponent>(Action<TComponent> initializer = null)
            where TComponent : IComponent, new()
        {
            var component = new TComponent();

            initializer?.Invoke(component);

            Add(component);

            return component;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public abstract TComponent Get<TComponent>() where TComponent : class, IComponent;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public abstract IReadOnlyCollection<TComponent> GetAll<TComponent>() where TComponent : class, IComponent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public abstract bool Has(IComponent component);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public abstract bool Has<TComponent>() where TComponent : IComponent;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public abstract IEnumerable<TComponent> Find<TComponent>() where TComponent : class, IComponent;

        /// <inheritdoc cref="GetState" />
        public abstract EntityState GetState();

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (Disposed)
            {
                return;
            }

            try
            {
                Dispose(true);
            }
            finally
            {
                Disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Entity CreateEntity(string key)
        {
            if (null == key)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("", nameof(key));
            }

            return new EntityImplementation(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Entity CreateRef(string key, Entity entity)
        {
            if (null == key)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("", nameof(key));
            }

            return new EntityReference(key, entity);
        }

        protected abstract void Dispose(bool dispose);
    }
}