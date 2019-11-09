using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LibraProgramming.Ecs.Core
{
    public partial class LiveComponentObserver
    {
        private sealed class MutualEntityEnumerator : IEnumerator<EntityBase>
        {
            private IList<EntityBase> collection;
            private EntityBase[] entities;
            private bool disposed;
            private int index;

            public EntityBase Current
            {
                get
                {
                    EnsureNotDisposed();
                    EnsureInitialized();

                    if (-1 == index)
                    {
                        throw new InvalidOperationException();
                    }

                    return entities[index];
                }
            }

            object IEnumerator.Current => Current;

            public MutualEntityEnumerator(IList<EntityBase> collection)
            {
                if (null == collection)
                {
                    throw new ArgumentNullException(nameof(collection));
                }

                this.collection = collection;

                entities = null;
                index = -1;
            }

            public bool MoveNext()
            {
                EnsureNotDisposed();
                
                if (-1 == index)
                {
                    entities = collection.ToArray();

                    if (0 < entities.Length)
                    {
                        index = 0;
                        return true;
                    }

                    return false;
                }

                if (index < entities.Length)
                {
                    return ++index < entities.Length;
                }

                return false;
            }

            public void Reset()
            {
                EnsureNotDisposed();

                entities = null;
                index = -1;
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
                        collection = null;
                        entities = null;
                    }
                }
                finally
                {
                    disposed = true;
                }
            }

            private void EnsureNotDisposed()
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(nameof(MutualEntityEnumerator));
                }
            }

            private void EnsureInitialized()
            {
                if (null == entities)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}