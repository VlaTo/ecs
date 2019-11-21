using System;
using System.Collections.Generic;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EntityLoader
    {
        private class DefaultComponentRegistry : IComponentRegistry, IComponentResolver
        {
            private readonly Dictionary<string, Func<IComponent>> creators;

            /// <summary>
            /// 
            /// </summary>
            public DefaultComponentRegistry()
            {
                creators = new Dictionary<string, Func<IComponent>>(StringComparer.InvariantCulture);
            }

            /// <inheritdoc cref="IComponentRegistry.Add" />
            public void Add(string alias, Func<IComponent> create)
            {
                creators.Add(alias, create);
            }

            /// <inheritdoc cref="IComponentResolver.Resolve" />
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