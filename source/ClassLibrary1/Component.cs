using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Component : IComponent
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract ComponentState GetState();

        protected abstract void DoAttach();

        protected abstract void DoRelease();
    }
}