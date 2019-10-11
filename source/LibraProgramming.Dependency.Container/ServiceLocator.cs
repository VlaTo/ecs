using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;

namespace LibraProgramming.Dependency.Container
{
    /// <summary>
    /// Implementation of the IOC pattern.
    /// </summary>
    public sealed partial class ServiceLocator : IServiceLocator, IInstanceProvider, IServiceRegistry
    {
        private static readonly Lazy<ServiceLocator> instance;
        private static readonly object sync;

        private readonly Dictionary<Type, InstanceCollection> registration;

        /// <summary>
        /// Gets instance of the <see cref="ServiceLocator" /> class.
        /// </summary>
        public static ServiceLocator Current => instance.Value;

        private ServiceLocator()
        {
            registration = new Dictionary<Type, InstanceCollection>();
        }

        static ServiceLocator()
        {
            sync = new object();
            instance = new Lazy<ServiceLocator>(() => new ServiceLocator());
        }

        #region Service Locator implementation

        /// <inheritdoc cref="IServiceProvider.GetService" />
        public object GetService(Type serviceType) => GetInstance(serviceType);

        /// <inheritdoc cref="IServiceLocator.GetInstance" />
        public object GetInstance(Type serviceType, string key = null)
        {
            if (null == serviceType)
            {
                Throw.ArgumentNull(nameof(serviceType));
            }

            var queue = new Queue<ServiceTypeReference>();

            return GetInstanceInternal(queue, serviceType, key);
        }

        /// <inheritdoc cref="IInstanceProvider.GetInstance" />
        object IInstanceProvider.GetInstance(Queue<ServiceTypeReference> queue, Type serviceType, string key)
            => GetInstanceInternal(queue, serviceType, key);

        /// <inheritdoc cref="IServiceLocator.GetInstance{TService}" />
        public TService GetInstance<TService>(string key = null) => (TService) GetInstance(typeof(TService), key);

        /// <inheritdoc cref="IServiceLocator.GetInstances" />
        public IEnumerable<object> GetInstances(Type serviceType)
        {
            if (null == serviceType)
            {
                Throw.ArgumentNull(nameof(serviceType));
            }

            lock (sync)
            {
                if (false == registration.TryGetValue(serviceType, out var collection))
                {
                    Throw.MissingServiceRegistration(serviceType, nameof(serviceType));
                }

                var instances = new Collection<object>();

                foreach (var lifetime in collection)
                {
                    var queue = new Queue<ServiceTypeReference>();
                    instances.Add(lifetime.ResolveInstance(queue));
                }

                return instances.AsEnumerable();
            }
        }

        /// <inheritdoc cref="IServiceLocator.CreateInstance" />
        public object CreateInstance(Type serviceType)
        {
            if (null == serviceType)
            {
                Throw.ArgumentNull(nameof(serviceType));
            }

            lock (sync)
            {
                var factory = new TypeFactory(this, serviceType);
                var manager = InstanceLifetime.CreateNew.Invoke(factory);
                var queue = new Queue<ServiceTypeReference>();

                return manager.ResolveInstance(queue);
            }
        }

        /// <inheritdoc cref="IServiceLocator.CreateInstance{TService}" />
        public TService CreateInstance<TService>() where TService : class
        {
            return (TService) CreateInstance(typeof(TService));
        }

        #endregion

        #region Service Registration

        /// <inheritdoc cref="IServiceRegistry.Register(Type, Func{Factory, InstanceLifetime}, string, bool)" />
        public void Register(Type service, Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false)
        {
            if (null == service)
            {
                Throw.ArgumentNull(nameof(service));
            }

            var typeInfo = service.GetTypeInfo();

            if (typeInfo.IsAbstract || typeInfo.IsInterface)
            {
                Throw.UnsupportedServiceType(service);
            }

            RegisterService(service, new TypeFactory(this, service), lifetime, key, createimmediate);
        }

        /// <inheritdoc cref="IServiceRegistry.Register(Type, Type, Func{Factory, InstanceLifetime}, string, bool)" />
        public void Register(Type service, Type impl, Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false)
        {
            if (null == service)
            {
                Throw.ArgumentNull(nameof(service));
            }

            var typeInfo = service.GetTypeInfo();

            if (!typeInfo.IsAbstract && !typeInfo.IsInterface)
            {
                Throw.UnsupportedServiceType(service);
            }

            if (null == impl)
            {
                Throw.ArgumentNull(nameof(impl));
            }

            typeInfo = impl.GetTypeInfo();

            if (typeInfo.IsAbstract || typeInfo.IsInterface)
            {
                Throw.UnsupportedServiceType(impl);
            }

            RegisterService(service, new TypeFactory(this, impl), lifetime, key, createimmediate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="factory"></param>
        /// <param name="lifetime"></param>
        /// <param name="key"></param>
        /// <param name="createimmediate"></param>
        public void Register<TService>(Func<TService> factory, Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false)
        {
            RegisterService(typeof (TService), new CreatorFactory<TService>(this, factory), lifetime, key, createimmediate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="lifetime"></param>
        /// <param name="key"></param>
        /// <param name="createimmediate"></param>
        public void Register<TService>(Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false) where TService : class 
        {
            Register(typeof (TService), lifetime, key, createimmediate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <param name="lifetime"></param>
        /// <param name="key"></param>
        /// <param name="createimmediate"></param>
        public void Register<TService, TConcrete>(Func<Factory, InstanceLifetime> lifetime = null, string key = null, bool createimmediate = false)
            where TConcrete : class, TService
        {
            Register(typeof (TService), typeof (TConcrete), lifetime, key, createimmediate);
        }

        private object GetInstanceInternal(Queue<ServiceTypeReference> queue, Type serviceType, string key = null)
        {
            lock (sync)
            {
                if (!registration.TryGetValue(serviceType, out var collection))
                {
                    Throw.MissingServiceRegistration(serviceType, nameof(serviceType));
                }

                return collection[key].ResolveInstance(queue);
            }
        }

        #endregion

        private void RegisterService(Type service, Factory factory, Func<Factory, InstanceLifetime> lifetime, string key, bool createimmediate)
        {
            lock (sync)
            {
                if (false == registration.TryGetValue(service, out var collection))
                {
                    collection = new InstanceCollection();
                    registration.Add(service, collection);
                }
                else if (null == key)
                {
                    Throw.MissingServiceRegistration(service, nameof(service));
                }

                var manager = lifetime ?? InstanceLifetime.Singleton;

                collection[key] = manager.Invoke(factory);

                if (createimmediate)
                {
                    var queue = new Queue<ServiceTypeReference>();
                    collection[key].ResolveInstance(queue);
                }
            }
        }
    }
}