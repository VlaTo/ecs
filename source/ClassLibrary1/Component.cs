using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public class Component : IComponent
    {
        /// <inheritdoc />
        public Entity Entity
        {
            get;
            private set;
        }

        /// <inheritdoc cref="IComponent.Attach" />
        public void Attach(Entity entity)
        {
            if (ReferenceEquals(Entity, entity))
            {
                return;
            }

            if (null != Entity)
            {
                throw new InvalidOperationException();
            }

            Entity = entity;

            if (null != Entity)
            {
                Entity.AddComponent(this);
            }
        }

        /// <inheritdoc cref="IComponent.Release" />
        public void Release()
        {
            if (null == Entity)
            {
                return;
            }

            Entity.RemoveComponent(this);

            Entity = null;
        }
    }
}