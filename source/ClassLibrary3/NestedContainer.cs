using System;
using System.ComponentModel;

namespace ClassLibrary3
{
    public class NestedContainer : Container, INestedContainer
    {
        public const char PathDelimiter = '/';

        public IComponent Owner
        {
            get;
        }

        protected virtual string OwnerName
        {
            get
            {
                string ownerName = null;

                if (null != Owner?.Site)
                {
                    ownerName = Owner.Site is INestedSite nestedOwnerSite ? nestedOwnerSite.FullName : Owner.Site.Name;
                }

                return ownerName;
            }
        }

        public NestedContainer(IComponent owner)
        {
            if (null == owner)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            Owner = owner;
            Owner.Disposed += OnOwnerDisposed;
        }

        protected override ISite CreateSite(IComponent component, string name)
        {
            if (null == component)
            {
                throw new ArgumentNullException(nameof(component));
            }

            return new Site(this, component, name);
        }

        protected override object GetService(Type service) =>
            service == typeof(INestedContainer) ? this : base.GetService(service);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Owner.Disposed -= OnOwnerDisposed;
            }

            base.Dispose(disposing);
        }

        private void OnOwnerDisposed(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private class Site : INestedSite
        {
            private readonly NestedContainer container;
            private string name;

            public IComponent Component
            {
                get;
            }

            public IContainer Container => container;

            public bool DesignMode
            {
                get
                {
                    var owner = container.Owner;
                    return null != owner?.Site && owner.Site.DesignMode;
                }
            }

            public string Name
            {
                get => name;
                set
                {
                    if (null == value || null == name || false == value.Equals(name))
                    {
                        container.ValidateName(Component, value);
                        name = value;
                    }
                }
            }

            public string FullName
            {
                get
                {
                    if (null != name)
                    {
                        var ownerName = container.OwnerName;
                        var childName = name;

                        if (null != ownerName)
                        {
                            var separator = PathDelimiter.ToString();
                            childName = String.Join(separator, ownerName, childName);
                        }

                        return childName;
                    }

                    return name;
                }
            }

            internal Site(NestedContainer container, IComponent component, string name)
            {
                this.container = container;
                Component = component;
                this.name = name;
            }

            public object GetService(Type service) => 
                service == typeof(ISite) ? this : container.GetService(service);
        }
    }
}