using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;
using System;

namespace ConsoleApp2.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class DependencyProviderAdapter : IDependencyProvider
    {
        private readonly ServiceLocator serviceLocator;

        public DependencyProviderAdapter(ServiceLocator serviceLocator)
        {
            if (null == serviceLocator)
            {
                throw new ArgumentNullException(nameof(serviceLocator));
            }

            this.serviceLocator = serviceLocator;
        }

        public object GetService(Type serviceType) => serviceLocator.GetService(serviceType);

        public ISystem CreateSystem(Type systemType) => (ISystem) serviceLocator.CreateInstance(systemType);
    }
}