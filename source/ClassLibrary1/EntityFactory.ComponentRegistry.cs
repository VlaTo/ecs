using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public partial class EntityFactory
    {
        private class DefaultComponentRegistry : IComponentRegistry, IComponentResolver
        {
            private readonly Dictionary<string, Func<IComponent>> creators;

            public DefaultComponentRegistry()
            {
                creators = new Dictionary<string, Func<IComponent>>(StringComparer.InvariantCulture);
            }

            public void Add(string alias, Func<IComponent> create)
            {
                creators.Add(alias, create);
            }

            public IComponent Resolve(string alias)
            {
                if (false == creators.TryGetValue(alias, out var create))
                {
                    throw new Exception();
                }

                return create.Invoke();
            }
        }
    }
}