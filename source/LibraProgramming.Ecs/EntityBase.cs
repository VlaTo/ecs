﻿using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Core.Path;
using LibraProgramming.Ecs.Core.Path.Segments;
using LibraProgramming.Ecs.Core.Reactive.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityBase : IEntity, IObservableCollection<IComponent>, ICloneable<EntityBase>
    {
        private EntityBase parent;
        private EntityPath path;

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
            private set;
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
        public EntityPath Path
        {
            get
            {
                if (null == path)
                {
                    var current = this;
                    EntityPathSegment last = null;

                    while (Root != current)
                    {
                        last = new EntityKey(current.Key, last);
                        current = current.Parent;
                    }

                    if (Root == current)
                    {
                        last = new PathRoot(last);
                    }

                    path = new EntityPath(last);
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
        public abstract void RemoveAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchPath"></param>
        /// <returns></returns>
        public virtual EntityBase Find(EntityPath searchPath)
        {
            if (null == searchPath)
            {
                throw new ArgumentNullException(nameof(searchPath));
            }

            var searcher = new EntityPathSearcher(this);
            var result = searcher.Find(searchPath);

            return result.IsSuccess ? result.Entity : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchPath"></param>
        /// <returns></returns>
        public virtual IEnumerable<EntityBase> FindAll(EntityPath searchPath)
        {
            if (null == searchPath)
            {
                throw new ArgumentNullException(nameof(searchPath));
            }

            var searcher = new EntityPathSearcher(this);
            var result = searcher.Find(searchPath);

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

        /// <inheritdoc cref="ICloneable{T}.Clone" />
        public abstract EntityBase Clone();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        internal void FixChildEntityKey(EntityBase entity)
        {
            var maxNumber = 0;

            foreach (var child in Children)
            {
                if (ReferenceEquals(child, entity))
                {
                    continue;
                }

                if (child.Key.StartsWith(entity.Key))
                {
                    if (child.Key.Length == entity.Key.Length)
                    {
                        maxNumber++;
                        continue;
                    }

                    var suffix = child.Key.Substring(entity.Key.Length);

                    if (int.TryParse(suffix, out var number))
                    {
                        maxNumber = Math.Max(maxNumber + 1, number);
                    }
                }
            }

            if (maxNumber > 0)
            {
                var key = entity.Key + maxNumber;
                entity.Key = key;
            }
        }
    }
}