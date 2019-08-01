using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ClassLibrary1.Core
{
    /// <summary>
    /// Observable collection of the <see cref="EntityBase" /> items.
    /// </summary>
    public interface IEntityCollection : IList<EntityBase>, IObservableCollection<EntityBase>
    {
        int FindIndex(Predicate<string> condition);

        void ForEach(Action<EntityBase> action);
    }

    /// <summary>
    /// 
    /// </summary>
    public class EntityCollection : IEntityCollection
    {
        private readonly EntityBase owner;
        private readonly ArrayList items;
        private readonly CollectionSubject<EntityBase> observers;

        private volatile int version;

        public int Count => items.Count;

        public bool IsReadOnly => items.IsReadOnly;

        public EntityBase this[int index]
        {
            get
            {
                if (0 > index || index >= items.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                return (EntityBase) items[index];
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

                observers.OnRemoved((EntityBase) items[index], index);

                items[index] = value;

                value.Parent = owner;

                observers.OnAdded(value, index);
            }
        }

        public EntityCollection(EntityBase owner)
        {
            this.owner = owner;

            //observers = new Collection<ICollectionObserver<EntityBase>>();
            observers = new CollectionSubject<EntityBase>();
            items = new ArrayList();
        }

        public void Add(EntityBase item)
        {
            Insert(Count, item);
        }

        public bool Contains(EntityBase item) => items.Contains(item);

        public void CopyTo(EntityBase[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public void Clear()
        {
            Interlocked.Increment(ref version);

            while (0 < items.Count)
            {
                var index = items.Count - 1;
                var item = (EntityBase) items[index];

                items.RemoveAt(index);

                observers.OnRemoved(item, index);
            }
        }

        public IEnumerator<EntityBase> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int FindIndex(Predicate<string> condition)
        {
            if (null == condition)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            for (var index = 0; index < items.Count; index++)
            {
                var entity = (EntityBase) items[index];

                if (condition.Invoke(entity.Key))
                {
                    return index;
                }
            }

            return -1;
        }

        public void ForEach(Action<EntityBase> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            for (var index = 0; index < items.Count; index++)
            {
                action.Invoke((EntityBase) items[index]);
            }
        }

        public int IndexOf(EntityBase item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return items.IndexOf(item);
        }

        public void Insert(int index, EntityBase item)
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

            observers.OnAdded(item, index);
        }

        public bool Remove(EntityBase item)
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

            var item = (EntityBase) items[index];

            items.RemoveAt(index);

            item.Parent = null;

            observers.OnRemoved(item, index);
        }

        public IDisposable Subscribe(ICollectionObserver<EntityBase> observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var disposable = observers.Subscribe(observer);

            for (var index = 0; index < items.Count; index++)
            {
                var entity = (EntityBase) items[index];
                observer.OnAdded(entity, index);
            }

            return disposable;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Enumerator : IEnumerator<EntityBase>
        {
            private readonly int version;
            private EntityCollection collection;
            private bool disposed;
            private int index;

            public EntityBase Current
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
    }
}