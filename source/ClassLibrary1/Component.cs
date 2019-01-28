using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Component : IComponent
    {
        private static readonly ComponentResolverCollection resolvers;

        /// <inheritdoc cref="IComponent.Alias" />
        public abstract string Alias
        {
            get;
        }

        /// <inheritdoc />
        public Entity Entity
        {
            get;
            private set;
        }

        public static IComponentResolverCollection Resolvers => resolvers;

        static Component()
        {
            resolvers = new ComponentResolverCollection();
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

        /// <inheritdoc cref="IStateProvider{TState}.GetState" />
        public abstract ComponentState GetState();

        /// <inheritdoc cref="IStateAcceptor{TState}.SetState" />
        public abstract void SetState(ComponentState state);

        /// <inheritdoc cref="ICloneable{T}.Clone" />
        public abstract IComponent Clone();

        protected abstract void DoAttach();

        protected abstract void DoRelease();
    }
}