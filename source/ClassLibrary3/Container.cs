using System;
using System.ComponentModel;

namespace ClassLibrary3
{
    public class Container : IContainer
    {
        private readonly object gate;
        private ISite[] sites;
        private int sitesCount;
        private ComponentCollection components;
        private ContainerFilterService filter;
        private bool checkedFilter;

        public ComponentCollection Components
        {
            get
            {
                lock (gate)
                {
                    if (null == components)
                    {
                        var result = new IComponent[sitesCount];

                        for (var index = 0; index < sitesCount; index++)
                        {
                            result[index] = sites[index].Component;
                        }

                        components = new ComponentCollection(result);

                        // At each component add, if we don't yet have a filter, look for one. 
                        // Components may add filters.
                        if (null == filter && checkedFilter)
                        {
                            checkedFilter = false;
                        }
                    }

                    if (false == checkedFilter)
                    {
                        filter = GetService(typeof(ContainerFilterService)) as ContainerFilterService;
                        checkedFilter = true;
                    }

                    if (null != filter)
                    {
                        var filteredComponents = filter.FilterComponents(components);

                        if (null != filteredComponents)
                        {
                            components = filteredComponents;
                        }
                    }

                    return components;
                }
            }
        }

        public Container()
        {
            gate = new object();
        }

        public void Add(IComponent component) => Add(component, null);

        public void Add(IComponent component, string name)
        {
            lock (gate)
            {
                AddComponent(component, name);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Remove(IComponent component)
        {
            lock (gate)
            {
                RemoveComponent(component, false);
            }
        }

        protected virtual ISite CreateSite(IComponent component, string name) => 
            new Site(this, component, name);

        protected virtual object GetService(Type serviceType) =>
            typeof(IContainer) == serviceType ? this : null;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (gate)
                {
                    while (0 < sitesCount)
                    {
                        var site = sites[--sitesCount];

                        site.Component.Site = null;
                        site.Component.Dispose();
                    }

                    sites = null;
                    components = null;
                }
            }
        }

        /// <devdoc> 
        ///     Validates that the given name is valid for a component.  The default implementation
        ///     verifies that name is either null or unique compared to the names of other 
        ///     components in the container.
        /// </devdoc>
        protected virtual void ValidateName(IComponent component, string name)
        {
            if (null == component)
            {
                throw new ArgumentNullException(nameof(component));
            }

            if (null == name)
            {
                return;
            }

            var condition = new Predicate<ISite>(site =>
                null != site?.Name && String.Equals(site.Name, name, StringComparison.OrdinalIgnoreCase) && component != site.Component
            );

            for (var index = 0; index < Math.Min(sitesCount, sites.Length); index++)
            {
                var site = sites[index];

                if (false == condition.Invoke(site))
                {
                    continue;
                }

                var inheritanceAttribute = (InheritanceAttribute) TypeDescriptor.GetAttributes(site.Component)[typeof(InheritanceAttribute)];

                if (inheritanceAttribute.InheritanceLevel != InheritanceLevel.InheritedReadOnly)
                {
                    throw new ArgumentException("", nameof(name));
                }
            }
        }

        private void AddComponent(IComponent component,string name)
        {
            var site = component.Site;

            if (null != site && this == site.Container)
            {
                return;
            }

            if (null == sites)
            {
                sites = new ISite[4];
            }
            else
            {
                // Validate that new components 
                // have either a null name or a unique one.
                // 
                ValidateName(component, name);

                if (sitesCount == sites.Length)
                {
                    var newSites = new ISite[sitesCount * 2];
                    Array.Copy(sites, 0, newSites, 0, sitesCount);
                    sites = newSites;
                }
            }

            if (null != site)
            {
                site.Container.Remove(component);
            }

            var newSite = CreateSite(component, name);

            sites[sitesCount++] = newSite;
            component.Site = newSite;
            components = null;
        }

        private void RemoveComponent(IComponent component, bool preserveSite)
        {
            if (null == component)
            {
                return;
            }

            var site = component.Site;

            if (null == site || this != site.Container)
            {
                return;
            }

            if (false == preserveSite)
            {
                component.Site = null;
            }

            for (var index = 0; index < sitesCount; index++)
            {
                if (site != sites[index])
                {
                    continue;
                }

                sitesCount--;
                Array.Copy(sites, index + 1, sites, index, sitesCount - index);
                sites[sitesCount] = null;
                components = null;

                break;
            }
        }

        private class Site : ISite
        {
            private readonly Container container;
            private string name;
            
            // The component sited by this component site.
            public IComponent Component
            {
                get;
            }

            // The container in which the component is sited.
            public IContainer Container => container;

            public bool DesignMode => false;
            
            // The name of the component. 
            //
            public String Name
            {
                get => name;
                set
                {
                    if (null == value || null == name || !value.Equals(name))
                    {
                        // 
                        container.ValidateName(Component, value);
                        name = value;
                    }
                }
            }

            internal Site(Container container, IComponent component, string name)
            {
                this.container = container;
                Component = component;
                this.name = name;
            }

            public object GetService(Type serviceType) =>
                typeof(ISite) == serviceType ? this : container.GetService(serviceType);
        }
    }
}