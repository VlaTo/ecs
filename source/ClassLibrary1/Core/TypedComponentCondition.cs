using System;

namespace ClassLibrary1.Core
{
    public class TypedComponentCondition<TComponent> : ICondition<IComponent>
        where TComponent : IComponent
    {
        private readonly Type componentType;

        public TypedComponentCondition()
        {
            componentType = typeof(TComponent);
        }

        public bool IsMet(IComponent value)
        {
            return value.GetType().IsAssignableFrom(componentType);
        }
    }
}