using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace LibraProgramming.Ecs.Core
{
    public sealed class ComponentResolver : IComponentResolver
    {
        private readonly IReadOnlyDictionary<string, Type> components;

        private ComponentResolver(IReadOnlyDictionary<string,Type> components)
        {
            this.components = components;
        }

        public static ComponentResolver FromAssemblies(Assembly[] assemblies)
        {
            if (null == assemblies)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            var components = new Dictionary<string, Type>(StringComparer.InvariantCulture);

            foreach (var assembly in assemblies)
            {
                RegisterComponentTypes(components, assembly.GetExportedTypes());
            }

            return new ComponentResolver(components);
        }

        public IComponent Resolve(string alias)
        {
            if (null == alias)
            {
                throw new ArgumentNullException(nameof(alias));
            }

            if (components.TryGetValue(alias, out var componentType))
            {
                return (IComponent) Activator.CreateInstance(componentType);
            }

            throw new InvalidOperationException(alias);
        }

        private static void RegisterComponentTypes(IDictionary<string,Type> components, Type[] sourceTypes)
        {
            var componentBaseType = typeof(Component);

            foreach (var sourceType in sourceTypes)
            {
                if (false == componentBaseType.IsAssignableFrom(sourceType))
                {
                    continue;
                }

                if (sourceType == componentBaseType)
                {
                    continue;
                }

                var alias = Component.GetComponentAlias(sourceType);

                components[alias] = sourceType;

                Debug.WriteLine($"Registering component: \'{alias}\' to type: \'{sourceType.FullName}\'");
            }
        }
    }
}