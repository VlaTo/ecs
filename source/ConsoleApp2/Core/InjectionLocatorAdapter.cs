using ClassLibrary1.Core;
using LibraProgramming.Dependency.Container;
using System;

namespace ConsoleApp2.Core
{
    public class InjectionLocatorAdapter : IDependencyInjection
    {
        private readonly ServiceLocator serviceLocator;

        public InjectionLocatorAdapter(ServiceLocator serviceLocator)
        {
            if (null == serviceLocator)
            {
                throw new ArgumentNullException(nameof(serviceLocator));
            }

            this.serviceLocator = serviceLocator;
        }

        public object GetService(Type serviceType) => serviceLocator.GetService(serviceType);

        public void Register(Type serviceType)
        {
            serviceLocator.Register(serviceType);
        }
    }
}