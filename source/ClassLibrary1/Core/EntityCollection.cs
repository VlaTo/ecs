using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace ClassLibrary1.Core
{
    public class EntityCollection : IEntityCollection
    {
        private readonly Entity owner;
        private readonly ArrayList items;
        private readonly ICollection<ICollectionObserver<Entity>> observers;
        private volatile int version;

        public int Count => items.Count;

        public bool IsReadOnly => items.IsReadOnly;

        public Entity this[int index]
        {
            get
            {
                if (0 > index || index >= items.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                return (Entity) items[index];
            }
            set
            {
                if (0 > index || index >= items.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                if (ReferenceEquals(items[index], value))
                {
                    return;
                }

                var previous = (Entity) items[index];

                items[index] = value;

                value.Parent = owner;

                DoItemChanged(index, previous, value);
            }
        }

        public EntityCollection(Entity owner)
        {
            this.owner = owner;

            observers = new Collection<ICollectionObserver<Entity>>();
            items = new ArrayList();
        }

        public void Add(Entity item)
        {
            Insert(Count, item);
        }

        public bool Contains(Entity item) => items.Contains(item);

        public void CopyTo(Entity[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public void Clear()
        {
            Interlocked.Increment(ref version);

            while (0 < items.Count)
            {
                var index = items.Count - 1;
                var item = (Entity) items[index];

                items.RemoveAt(index);

                DoItemRemoved(index, item);
            }
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(Entity item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.IndexOf(item);
        }

        public void Insert(int index, Entity item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (0 > index || index > Count)
            {
                throw new IndexOutOfRangeException();
            }

            if (items.Contains(item))
            {
                return;
            }

            Interlocked.Increment(ref version);

            items.Insert(index, item);

            item.Parent = owner;

            DoItemAdded(index, item);
        }

        public bool Remove(Entity item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = items.IndexOf(item);

            if (0 > index)
            {
                return false;
            }

            RemoveAt(index);

            return true;
        }

        public void RemoveAt(int index)
        {
            if (0 > index || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            Interlocked.Increment(ref version);

            var item = (Entity) items[index];

            items.RemoveAt(index);

            item.Parent = null;

            DoItemRemoved(index, item);
        }

        public IDisposable Subscribe(ICollectionObserver<Entity> observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            if (false == observers.Contains(observer))
            {
                observers.Add(observer);
            }

            for (var index = 0; index < items.Count; index++)
            {
                var entity = (Entity) items[index];
                observer.OnAdded(entity, index);
            }

            return new Subscription(this, observer);
        }

        private void Unsubscribe(ICollectionObserver<Entity> observer)
        {
            if (observers.Remove(observer))
            {
                observer.OnCompleted();
            }
        }

        private void DoItemAdded(int index, Entity item)
        {
            foreach (var observer in observers)
            {
                observer.OnAdded(item, index);
            }
        }

        private void DoItemChanged(int index, Entity oldValue, Entity newValue)
        {
            foreach (var observer in observers)
            {
                observer.OnRemoved(oldValue, index);
                observer.OnAdded(newValue, index);
            }
        }

        private void DoItemRemoved(int index, Entity item)
        {
            foreach (var observer in observers)
            {
                observer.OnRemoved(item, index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Enumerator : IEnumerator<Entity>
        {
            private readonly int version;
            private EntityCollection collection;
            private bool disposed;
            private int index;

            public Entity Current
            {
                get
                {
                    EnsureNotDisposed();
                    EnsureNotModified();

                    return collection[index];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(EntityCollection collection)
            {
                this.collection = collection;
                version = collection.version;
                index = -1;
            }

            public bool MoveNext()
            {
                EnsureNotDisposed();
                EnsureNotModified();

                var count = collection.Count;

                if (-1 == index)
                {
                    if (0 == count)
                    {
                        return false;
                    }

                    index = 0;

                    return true;
                }

                if (count <= index)
                {
                    return false;
                }

                return count > ++index;
            }

            public void Reset()
            {
                EnsureNotDisposed();
                EnsureNotModified();

                index = -1;
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void EnsureNotDisposed()
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(Enumerator));
                }
            }

            private void EnsureNotModified()
            {
                if (version != collection.version)
                {
                    throw new InvalidOperationException();
                }
            }

            private void Dispose(bool dispose)
            {
                if (disposed)
                {
                    return;
                }

                try
                {
                    if (dispose)
                    {
                        collection = null;
                    }
                }
                finally
                {
                    disposed = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly EntityCollection collection;
            private readonly ICollectionObserver<Entity> observer;
            private bool disposed;

            public Subscription(EntityCollection collection, ICollectionObserver<Entity> observer)
            {
                this.collection = collection;
                this.observer = observer;
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool dispose)
            {
                if (disposed)
                {
                    return;
                }

                try
                {
                    if (dispose)
                    {
                        collection.Unsubscribe(observer);
                    }
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}