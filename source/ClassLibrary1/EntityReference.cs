using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityReference : Entity
    {
        private Entity entity;

        /// <summary>
        /// 
        /// </summary>
        public EntityPathString EntityPath
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        internal Entity Entity => entity ?? (entity = ResolveEntity());

        /// <inheritdoc cref="Children" />
        public override IEntityCollection Children => entity.Children;

        /// <inheritdoc cref="Entity.Components" />
        public override IEnumerable<IComponent> Components => entity.Components;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entityPath"></param>
        public EntityReference(string key, EntityPathString entityPath)
            : base(key)
        {
            EntityPath = entityPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        public EntityReference(string key, Entity entity)
            : base(key)
        {
            EntityPath = entity.Path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        public EntityReference(EntityReference instance)
            : this(instance.Key, instance.EntityPath)
        {
        }

        /// <inheritdoc cref="ClassLibrary1.Entity.Subscribe" />
        public override IDisposable Subscribe(IEntityObserver observer)
        {
            return Entity.Subscribe(observer);
        }

        /// <inheritdoc cref="ClassLibrary1.Entity.Add" />
        public override void Add(IComponent component)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="ClassLibrary1.Entity.Remove" />
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

        public override IEnumerable<Entity> Find(EntityPathString path)
        {
            return Entity.Find(path);
        }

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
                EntityPath = (string) EntityPath
            };
        }

        public override void SetState(EntityState state)
        {
            throw new NotImplementedException();
        }

        public override Entity Clone()
        {
            throw new NotImplementedException();
        }

        private Entity ResolveEntity()
        {
            //var match = new EntityPathMatch(EntityPath, this);
            //return match.IsMet()
            throw new NotImplementedException();
        }
    }
}