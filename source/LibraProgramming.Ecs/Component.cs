using System;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Component : IComponent
    {
        /// <inheritdoc />
        public EntityBase Entity
        {
            get;
            private set;
        }

        /// <inheritdoc cref="IComponent.Attach" />
        public void Attach(EntityBase entity)
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

            if (null == Entity)
            {
                return;
            }

            Entity.Add(this);

            DoAttach();
        }

        /// <inheritdoc cref="IComponent.Release" />
        public void Release()
        {
            if (null == Entity)
            {
                return;
            }

            Entity.Remove(this);

            Entity = null;

            DoRelease();
        }

        /// <inheritdoc cref="ICloneable{T}.Clone" />
        public abstract IComponent Clone();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void DoAttach();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void DoRelease();
    }
}