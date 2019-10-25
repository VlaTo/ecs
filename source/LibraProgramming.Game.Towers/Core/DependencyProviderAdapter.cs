using System;
using LibraProgramming.Dependency.Container;
using LibraProgramming.Ecs;
using LibraProgramming.Ecs.Core;

namespace LibraProgramming.Game.Towers.Core
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