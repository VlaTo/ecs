using ClassLibrary1.Core.Extensions;
using ClassLibrary1.Core.Path;
using ClassLibrary1.Core.Path.Extensions;
using ClassLibrary1.Core.Path.Segments;
using ClassLibrary1.Core.Reactive.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using ClassLibrary1.Core.Reactive;

namespace ClassLibrary1.Core
{
    public class LiveComponentObserver<TComponent> : IEnumerable<TComponent>, IDisposable
        where TComponent : IComponent
    {
        private readonly LiveEntityCollectionWatcher watcher;
        private readonly IList<TComponent> components;
        private bool disposed;

        public static LiveComponentObserver<TComponent> Subscribe(EntityBase entity, EntityPath entityPath)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == entityPath)
            {
                throw new ArgumentNullException(nameof(entityPath));
            }

            var components = new List<TComponent>();

            void OnComponentAdded(IComponent item)
            {
                if (item is TComponent component)
                {
                    components.Add(component);
                }
            }

            void OnComponentRemoved(IComponent item)
            {
                if (item is TComponent component)
                {
                    if (components.Remove(component))
                    {
                        ;
                    }
                }
            }

            var temp = CollectionObserver.Create<IComponent>(OnComponentAdded, OnComponentRemoved);
            var observer = SubscribeInternal(entityPath.Entry, temp);
            var componentObserver = new LiveComponentObserver<TComponent>(observer, components);

            observer.Subscribe(entityPath.IsAbsolute() ? entity.Root : entity);

            return componentObserver;
        }

        private LiveComponentObserver(LiveEntityCollectionWatcher watcher, IList<TComponent> components)
        {
            this.watcher = watcher;
            this.components = components;
        }

        public IEnumerator<TComponent> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            Dispose(true);
        }

        private static LiveEntityCollectionWatcher SubscribeInternal(EntityPathSegment path, ICollectionObserver<IComponent> observer)
        {
            if (null == path)
            {
                return null;
            }

            var next = SubscribeInternal(path.Next, observer);

            if (path.IsRoot())
            {
                return new RootEntityCollectionWatcher(next, observer);
            }

            if (path.IsEntityKey(out var key))
            {
                return new KeyEntityCollectionWatcher(next, observer, key);
            }

            if (path.IsWildcard())
            {
                return new WildcardEntityCollectionWatcher(next, observer);
            }

            throw new InvalidOperationException();
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
                    ;
                }
            }
            finally
            {
                disposed = true;
            }
        }

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

            public ICollectionObserver<IComponent> Observer
            {
                get;
            }

            protected LiveEntityCollectionWatcher(
                LiveEntityCollectionWatcher next, 
                ICollectionObserver<IComponent> observer)
            {
                Next = next;
                Observer = observer;
            }

            public abstract void Subscribe(EntityBase entity);

            public virtual void OnError(Exception error)
            {
                Next?.OnError(error);
            }

            public virtual void OnCompleted()
            {
                Next?.OnCompleted();
            }

            public abstract void OnAdded(EntityBase item);

            public abstract void OnRemoved(EntityBase item);

            public void Dispose()
            {
                Dispose(true);
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

        /// <summary>
        /// 
        /// </summary>
        private class RootEntityCollectionWatcher : LiveEntityCollectionWatcher
        {
            private IDisposable disposable;
            private IDisposable subscription;

            public RootEntityCollectionWatcher(LiveEntityCollectionWatcher next, ICollectionObserver<IComponent> observer)
                : base(next, observer)
            {
                disposable = Disposable.Empty;
                subscription = Disposable.Empty;
            }

            public override void Subscribe(EntityBase entity)
            {
                EnsureNotDisposed();

                if (null == entity)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                if (false == ReferenceEquals(entity.Root, entity))
                {
                    throw new InvalidOperationException();
                }

                if (false == ReferenceEquals(Disposable.Empty, disposable))
                {
                    throw new InvalidOperationException();
                }

                if (null != Next)
                {
                    disposable = entity.Children.Subscribe(this);
                    return;
                }

                subscription = entity.Subscribe(Observer);
            }

            public override void OnAdded(EntityBase item)
            {
                EnsureNotDisposed();
                Next.Subscribe(item);
            }

            public override void OnRemoved(EntityBase item)
            {
                EnsureNotDisposed();
                disposable.Dispose();
            }

            protected override void OnDispose()
            {
                disposable.Dispose();
                subscription.Dispose();
                base.OnDispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class KeyEntityCollectionWatcher : LiveEntityCollectionWatcher
        {
            //private readonly Dictionary<EntityBase, IDisposable> entities;
            private readonly List<EntityBase> entities;
            private IDisposable disposable;
            private IDisposable subscription;

            public string EntityKey
            {
                get;
            }

            public KeyEntityCollectionWatcher(
                LiveEntityCollectionWatcher next,
                ICollectionObserver<IComponent> observer,
                string entityKey)
                : base(next, observer)
            {
                disposable = Disposable.Empty;
                subscription = Disposable.Empty;
                //entities = new Dictionary<EntityBase, IDisposable>();
                entities = new List<EntityBase>();

                EntityKey = entityKey;
            }

            public override void Subscribe(EntityBase entity)
            {
                EnsureNotDisposed();

                if (null == entity)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                if (false == ReferenceEquals(Disposable.Empty, disposable))
                {
                    throw new InvalidOperationException();
                }

                if (String.Equals(entity.Key, EntityKey))
                {
                    disposable = entity.Subscribe(Observer);

                    if (null != Next)
                    {
                        subscription = entity.Children.Subscribe(this);
                    }
                }
            }

            public override void OnAdded(EntityBase item)
            {
                EnsureNotDisposed();
                
                Next.Subscribe(item);
                
                entities.Add(item);
            }

            public override void OnRemoved(EntityBase item)
            {
                EnsureNotDisposed();

                if (entities.Remove(item))
                {
                    ;
                }
            }

            protected override void OnDispose()
            {
                foreach (var item in entities)
                {
                }

                disposable.Dispose();
                subscription.Dispose();

                base.OnDispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class WildcardEntityCollectionWatcher : LiveEntityCollectionWatcher
        {
            //private readonly Dictionary<EntityBase, IDisposable> subscriptions;
            private readonly List<EntityBase> entities;
            private IDisposable disposable;
            private IDisposable subscription;

            public WildcardEntityCollectionWatcher(
                LiveEntityCollectionWatcher next,
                ICollectionObserver<IComponent> observer)
                : base(next, observer)
            {
                disposable = Disposable.Empty;
                subscription = Disposable.Empty;
                //subscriptions = new Dictionary<EntityBase, IDisposable>();
                entities = new List<EntityBase>();
            }

            public override void Subscribe(EntityBase entity)
            {
                EnsureNotDisposed();

                if (null == entity)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                //disposable = entity.Children.Subscribe(this);
                disposable = entity.Subscribe(Observer);

                if (null != Next)
                {
                    subscription = entity.Children.Subscribe(this);
                }
            }

            public override void OnAdded(EntityBase item)
            {
                EnsureNotDisposed();

                Next.Subscribe(item);

                entities.Add(item);
            }

            public override void OnRemoved(EntityBase item)
            {
                EnsureNotDisposed();

                if (entities.Remove(item))
                {
                    ;
                }
            }

            protected override void OnDispose()
            {
                foreach (var item in entities)
                {
                    ;
                }

                subscription.Dispose();
                disposable.Dispose();

                base.OnDispose();
            }
        }
    }
}