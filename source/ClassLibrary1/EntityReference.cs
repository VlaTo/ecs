using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityReference : Entity
    {
        private readonly Entity entity;

        /// <inheritdoc cref="Children" />
        public override IEntityCollection Children => entity.Children;

        /// <inheritdoc cref="Entity.Components" />
        public override IEnumerable<IComponent> Components => entity.Components;

        internal EntityReference(string key, Entity entity)
            : base(key)
        {
            this.entity = entity;
        }

        public override IDisposable Subscribe(IEntityObserver observer)
        {
            return entity.Subscribe(observer);
        }

        public override void Add(IComponent component)
        {
            entity.Add(component);
        }

        public override void Remove(IComponent component)
        {
            entity.Remove(component);
        }

        public override TComponent Get<TComponent>()
        {
            return entity.Get<TComponent>();
        }

        public override IReadOnlyCollection<TComponent> GetAll<TComponent>()
        {
            return entity.GetAll<TComponent>();
        }

        public override bool Has(IComponent component)
        {
            return entity.Has(component);
        }

        public override bool Has<TComponent>()
        {
            return entity.Has<TComponent>();
        }

        public override IEnumerable<TComponent> Find<TComponent>()
        {
            return entity.Find<TComponent>();
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
        }
    }
}