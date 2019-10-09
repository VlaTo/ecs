using System;
using System.Collections.Generic;

namespace LibraProgramming.Dependency.Container
{
    /// <summary>
    /// 
    /// </summary>
    internal class TypeFactory : Factory
    {
        private readonly Type type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="type"></param>
        public TypeFactory(IInstanceProvider provider, Type type)
            : base(provider)
        {
            this.type = type;
        }

        /// <inheritdoc cref="Factory.Create" />
        public override object Create(Queue<ServiceTypeReference> types)
        {
            return CreateInstance(types, type);
        }
    }
}