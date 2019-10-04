using System;
using System.Collections.Generic;
using ClassLibrary1.Core;
using ClassLibrary1.Core.Path;
using ClassLibrary1.Core.Reactive.Collections;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ReferencedEntity : EntityBase
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
        internal EntityBase Entity => entity ?? (entity = ResolveEntity());

        /// <inheritdoc cref="Children" />
        public override IEntityCollection Children => entity.Children;

        /// <inheritdoc cref="Entity.Components" />
        public override IEnumerable<IComponent> Components => entity.Components;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entityPath"></param>
        public ReferencedEntity(string key, EntityPath entityPath)
            : base(key)
        {
            EntityPath = entityPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        public ReferencedEntity(string key, EntityBase entity)
            : base(key)
        {
            EntityPath = entity.Path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public ReferencedEntity(ReferencedEntity instance)
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

        /// <inheritdoc cref="GetState" />
        public override EntityState GetState()
        {
            return new EntityState
            {
                Key = Key,
                EntityPath = (string) EntityPath,
                IsReference = true
            };
        }

        public override EntityBase Clone()
        {
            throw new NotImplementedException();
        }

        private EntityBase ResolveEntity()
        {
            //var match = new EntityPathMatch(EntityPath, this);
            //return match.IsMet()
            throw new NotImplementedException();
        }
    }
}