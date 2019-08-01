using System;
using System.ComponentModel;

namespace ClassLibrary3
{
    public abstract class Component : IComponent
    {
        private static readonly object eventDisposed;

        private EventHandlerList events;

        public ISite Site
        {
            get;
            set;
        }

        public IContainer Container => Site?.Container;

        internal bool CanRaiseEventsInternal => CanRaiseEvents;

        protected virtual bool CanRaiseEvents => true;

        protected EventHandlerList Events => events ?? (events = new EventHandlerList(this));

        public event EventHandler Disposed
        {
            add => Events.AddHandler(eventDisposed, value);
            remove => Events.RemoveHandler(eventDisposed, value);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        static Component()
        {
            eventDisposed = new object();
        }

        public virtual object GetService(Type service) =>
            Site?.GetService(service);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this)
                {
                    if (null != Site?.Container)
                    {
                        Site.Container.Remove(this);
                    }

                    if (null != events)
                    {
                        var handler = (EventHandler) events[eventDisposed];

                        if (null != handler)
                        {
                            handler.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
            }
        }
    }
}