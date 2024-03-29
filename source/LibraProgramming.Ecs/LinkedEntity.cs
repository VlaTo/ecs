﻿using System;
using System.Collections.Generic;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Core.Path;
using LibraProgramming.Ecs.Core.Reactive.Collections;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LinkedEntity : EntityBase
    {
        private EntityBase entity;

        /// <summary>
        /// 
        /// </summary>
        public EntityPath EntityPath
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        internal EntityBase Entity => entity ?? (entity = Find(EntityPath));

        /// <inheritdoc cref="Children" />
        public override IEntityCollection Children => Entity.Children;

        /// <inheritdoc cref="Entity.Components" />
        public override IEnumerable<IComponent> Components => Entity.Components;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entityPath"></param>
        public LinkedEntity(string key, EntityPath entityPath)
            : base(key)
        {
            EntityPath = entityPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        public LinkedEntity(string key, EntityBase entity)
            : base(key)
        {
            EntityPath = entity.Path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public LinkedEntity(LinkedEntity instance)
            : this(instance.Key, instance.EntityPath)
        {
        }

        /// <inheritdoc cref="EntityBase.Subscribe" />
        public override IDisposable Subscribe(ICollectionObserver<IComponent> observer)
        {
            return Entity.Subscribe(observer);
        }

        /// <inheritdoc cref="EntityBase.Add" />
        public override void Add(IComponent component)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="EntityBase.Remove" />
        public override void Remove(IComponent component)
        {
            throw new NotSupportedException();
        }

        public override TComponent Get<TComponent>()
        {
            return Entity.Get<TComponent>();
        }

        public override IReadOnlyCollection<TComponent> GetAll<TComponent>()
        {
            return Entity.GetAll<TComponent>();
        }

        public override void RemoveAll()
        {
            throw new NotSupportedException();
        }

        /*public override IEnumerable<EntityBase> Find(EntityPathString path)
        {
            return Entity.Find(path);
        }*/

        public override bool Has(IComponent component)
        {
            return Entity.Has(component);
        }

        public override bool Has<TComponent>()
        {
            return Entity.Has<TComponent>();
        }

        /*public override EntityState GetState()
        {
            return new EntityState
            {
                Key = Key,
                EntityPath = (string) EntityPath,
                IsReference = true
            };
        }*/

        public override EntityBase Clone()
        {
            throw new NotImplementedException();
        }
    }
}