using System;
using LibraProgramming.Ecs.Core.Reactive.Collections;

namespace LibraProgramming.Ecs.Core
{
    public partial class LiveComponentObserver
    {
        /// <summary>
        /// 
        /// </summary>
        private abstract class LiveEntityCollectionWatcher : ICollectionObserver<EntityBase>, IDisposable
        {
            private bool disposed;

            public LiveEntityCollectionWatcher Next
            {
                get;
            }

            public IScopedCollectionObserver<IComponent, EntityBase> Observer
            {
                get;
            }

            protected LiveEntityCollectionWatcher(
                LiveEntityCollectionWatcher next,
                IScopedCollectionObserver<IComponent, EntityBase> observer)
            {
                Next = next;
                Observer = observer;
            }

            public abstract IDisposable Subscribe(EntityBase entity);

            void IError.OnError(Exception error)
            {
                DoError(error);
            }

            void ICompletable.OnCompleted()
            {
                DoCompleted();
            }

            void ICollectionObserver<EntityBase>.OnAdded(EntityBase item)
            {
                DoAdded(item);
            }

            void ICollectionObserver<EntityBase>.OnRemoved(EntityBase item)
            {
                DoRemoved(item);
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected abstract void DoAdded(EntityBase entity);

            protected abstract void DoRemoved(EntityBase entity);

            protected virtual void DoCompleted()
            {
                ((ICollectionObserver<EntityBase>) Next)?.OnCompleted();
            }

            protected virtual void DoError(Exception error)
            {
                ((ICollectionObserver<EntityBase>) Next)?.OnError(error);
            }

            protected void EnsureNotDisposed()
            {
                if (disposed)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }
            }

            protected virtual void OnDispose()
            {
                Next?.Dispose();
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
                        OnDispose();
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