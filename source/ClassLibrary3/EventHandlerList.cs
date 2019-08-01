using System;
using System.Diagnostics.CodeAnalysis;

namespace ClassLibrary3
{
    public sealed class EventHandlerList : IDisposable
    {
        private ListEntry head;
        private Component parent;

        public Delegate this[object key]
        {
            [SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
            get
            {
                ListEntry entry = null;

                if (null == parent || parent.CanRaiseEventsInternal)
                {
                    entry = Find(key);
                }

                return null != entry ? entry.Handler : null;
            }
            set
            {
                var e = Find(key);

                if (null != e)
                {
                    e.Handler = value;
                }
                else
                {
                    head = new ListEntry(key, value, head);
                }
            }
        }

        internal EventHandlerList(Component parent)
        {
            this.parent = parent;
        }

        public void AddHandler(object key, Delegate value)
        {
            var entry = Find(key);

            if (null != entry)
            {
                entry.Handler = Delegate.Combine(entry.Handler, value);
            }
            else
            {
                head = new ListEntry(key, value, head);
            }
        }

        public void AddHandlers(EventHandlerList listToAddFrom)
        {
            var currentListEntry = listToAddFrom.head;

            while (null != currentListEntry)
            {
                AddHandler(currentListEntry.Key, currentListEntry.Handler);
                currentListEntry = currentListEntry.Next;
            }
        }

        public void Dispose()
        {
            head = null;
        }

        public void RemoveHandler(object key, Delegate value)
        {
            var entry = Find(key);

            if (null != entry)
            {
                entry.Handler = Delegate.Remove(entry.Handler, value);
            }
        }

        private ListEntry Find(object key)
        {
            var found = head;

            while (null != found)
            {
                if (key == found.Key)
                {
                    break;
                }

                found = found.Next;
            }

            return found;
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class ListEntry
        {
            internal ListEntry Next
            {
                get;
            }

            internal object Key
            {
                get;
            }

            internal Delegate Handler
            {
                get;
                set;
            }

            public ListEntry(object key, Delegate handler, ListEntry next)
            {
                Next = next;
                Key = key;
                Handler = handler;
            }
        }
    }
}