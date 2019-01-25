using System;
using System.Collections.Generic;
using ClassLibrary1.Core;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ComponentResolverCollection : IComponentResolverCollection
    {
        private readonly IDictionary<string, ComponentResolver> resolvers;

        public ComponentResolverCollection()
        {
            resolvers = new Dictionary<string, ComponentResolver>(StringComparer.InvariantCulture);
        }

        /// <inheritdoc cref="IComponentResolver.Resolve" />
        public IComponent Resolve(string alias)
        {
            if (false == resolvers.TryGetValue(alias, out var resolver))
            {
                throw new EntityException();
            }

            return resolver.GetComponent();
        }
    }
}