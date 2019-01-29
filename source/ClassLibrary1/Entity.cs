using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Entity : IObservableEntity, IStateProvider<EntityState>, IStateAcceptor<EntityState>, ICloneable<Entity>
    {
        internal const char Separator = '/';

        private Entity parent;

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
        public virtual string Key
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
                var path = new StringBuilder();
                var root = Root;
                var current = this;

                while (root != current)
                {
                    if (0 < path.Length)
                    {
                        path.Insert(0, Separator);
                    }

                    path.Insert(0, current.Key);
                    current = current.Parent;
                }

                if (root == current)
                {
                    path.Insert(0, Separator);
                }

                return EntityPathString.Parse(path.ToString());
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
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract IEnumerable<Entity> Find(EntityPathString path);

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

        /// <inheritdoc cref="IStateProvider{TState}.GetState" />
        public abstract EntityState GetState();

        /// <inheritdoc cref="IStateAcceptor{TState}.SetState" />
        public abstract void SetState(EntityState state);

        /// <inheritdoc cref="ICloneable{T}.Clone" />
        public abstract Entity Clone();
    }
}