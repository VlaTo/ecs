using System;
using System.Collections.Generic;

namespace LibraProgramming.Dependency.Container
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class CreatorFactory<TService> : Factory
    {
        private readonly Func<TService> creator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="creator"></param>
        public CreatorFactory(IInstanceProvider provider, Func<TService> creator)
            : base(provider)
        {
            this.creator = creator;
        }

        /// <inheritdoc cref="Factory.Create" />
        public override object Create(Queue<ServiceTypeReference> types)
        {
            return creator.Invoke();
        }
    }
}