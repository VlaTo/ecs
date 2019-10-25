using System;
using System.Reflection;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Component : IComponent, IStateProvider<ComponentState>, IStateConsumer<ComponentState>
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

        public virtual ComponentState GetState()
        {
            var state = new ComponentState
            {
                Alias = GetComponentAlias(GetType())
            };

            DoFillState(state);

            return state;
        }

        public void ApplyState(ComponentState state)
        {
            if (null == state)
            {
                throw new ArgumentNullException(nameof(state));
            }

            var expectedAlias = GetComponentAlias(GetType());

            if (false == String.Equals(state.Alias, expectedAlias))
            {
                throw new InvalidOperationException();
            }

            DoApplyState(state);
        }

        /// <inheritdoc cref="ICloneable{T}.Clone" />
        public abstract IComponent Clone();

        internal static string GetComponentAlias(Type componentType)
        {
            if (null == componentType)
            {
                throw new ArgumentNullException(nameof(componentType));
            }

            var attribute = componentType.GetCustomAttribute<ComponentAttribute>();
            var componentName = componentType.Name;

            if (null == attribute)
            {
                return componentName;
            }

            if (String.IsNullOrEmpty(attribute.Alias))
            {
                const string suffix = nameof(Component);
                return componentName.EndsWith(suffix)
                    ? componentName.Substring(0, componentName.Length - suffix.Length)
                    : componentName;
            }

            return attribute.Alias;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DoAttach()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DoRelease()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        protected abstract void DoFillState(ComponentState state);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        protected abstract void DoApplyState(ComponentState state);
    }
}