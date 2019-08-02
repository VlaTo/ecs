﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary1.Core;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityBase : IEntity, IObservableCollection<IComponent>, IStateProvider<EntityState>, ICloneable<EntityBase>
    {
        private EntityBase parent;
        private EntityPathString path;

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
        public virtual EntityBase Parent
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
        public EntityPathString Path
        {
            get
            {
                if (null == path)
                {
                    var current = this;
                    EntityPathStringSegment last = null;

                    while (Root != current)
                    {
                        last = new StringEntityPathStringSegment(current.Key, last);
                        current = current.Parent;
                    }

                    if (Root == current)
                    {
                        last = new RootEntityPathStringSegment(last);
                    }

                    path = new EntityPathString(last);
                }

                return path;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual EntityBase Root
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

        protected EntityBase()
        {
        }

        protected EntityBase(string key)
            : this()
        {
            Key = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public abstract IDisposable Subscribe(ICollectionObserver<IComponent> observer);

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
        /// <param name="searchPath"></param>
        /// <returns></returns>
        public virtual EntityBase Find(EntityPathString searchPath)
        {
            if (null == searchPath)
            {
                throw new ArgumentNullException(nameof(searchPath));
            }

            var finder = new EntityPathFinder(this);
            var result = finder.Search(searchPath);

            return result.IsSuccess ? result.Entity : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchPath"></param>
        /// <returns></returns>
        public virtual IEnumerable<EntityBase> FindAll(EntityPathString searchPath)
        {
            if (null == searchPath)
            {
                throw new ArgumentNullException(nameof(searchPath));
            }

            var finder = new EntityPathFinder(this);
            var result = finder.Search(searchPath);

            return result.IsSuccess ? new[] {result.Entity} : Enumerable.Empty<EntityBase>();
        }

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

        /// <inheritdoc cref="ICloneable{T}.Clone" />
        public abstract EntityBase Clone();
    }
}