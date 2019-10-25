using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using LibraProgramming.Ecs.Core.Extensions;
using LibraProgramming.Ecs.Core.Path;
using LibraProgramming.Ecs.Core.Path.Extensions;
using LibraProgramming.Ecs.Core.Path.Segments;
using LibraProgramming.Ecs.Core.Reactive;
using LibraProgramming.Ecs.Core.Reactive.Collections;

namespace LibraProgramming.Ecs.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class LiveComponentObserver : IEnumerable<EntityBase>, IDisposable
    {
        private readonly LiveEntityCollectionWatcher watcher;
        private readonly IList<EntityBase> entities;
        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityPath"></param>
        /// <returns></returns>
        public static LiveComponentObserver Subscribe<TComponent>(EntityBase entity, EntityPath entityPath)
            where TComponent : IComponent
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == entityPath)
            {
                throw new ArgumentNullException(nameof(entityPath));
            }

            /*var entities = new List<EntityBase>();

            void OnComponentAdded(IComponent item)
            {
                if (item is TComponent component)
                {
                    entities.Add(component.Entity);
                }
            }

            void OnComponentRemoved(IComponent item)
            {
                if (item is TComponent component)
                {
                    if (entities.Remove(component.Entity))
                    {
                        ;
                    }
                }
            }

            var temp = CollectionObserver.Create<IComponent>(OnComponentAdded, OnComponentRemoved);*/

            var collector = new FilteredEntityCollector<TComponent>();
            var observer = SubscribeInternal(entityPath.Entry, collector);
            var componentObserver = new LiveComponentObserver(observer, collector.Entities);

            observer.Subscribe(entityPath.IsAbsolute() ? entity.Root : entity);

            return componentObserver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityPath"></param>
        /// <returns></returns>
        public static LiveComponentObserver Subscribe<TComponent1, TComponent2>(EntityBase entity, EntityPath entityPath)
            where TComponent1 : IComponent
            where TComponent2 : IComponent
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == entityPath)
            {
                throw new ArgumentNullException(nameof(entityPath));
            }

            /*var entities = new List<EntityBase>();

            void OnComponentAdded(IComponent item)
            {
                var target = item.Entity;

                
                if (target.Has<TComponent1>())
                {
                    entities.Add(target);
                }
            }

            void OnComponentRemoved(IComponent item)
            {
                var target = item.Entity;

                if (entities.Contains(target))
                {
                    if (item is TComponent1)
                    {
                        if (entities.Remove(target))
                        {
                            ;
                        }
                    }

                    if (item is TComponent2)
                    {
                        if (entities.Remove(target))
                        {
                            ;
                        }
                    }
                }
            }*/

            //var temp = CollectionObserver.Create<IComponent>(OnComponentAdded, OnComponentRemoved);

            var collector = new FilteredEntityCollector<TComponent1, TComponent2>();
            var observer = SubscribeInternal(entityPath.Entry, collector);
            var componentObserver = new LiveComponentObserver(observer, collector.Entities);

            observer.Subscribe(entityPath.IsAbsolute() ? entity.Root : entity);

            return componentObserver;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityPath"></param>
        /// <returns></returns>
        public static LiveComponentObserver Subscribe<TComponent1, TComponent2, TComponent3>(EntityBase entity, EntityPath entityPath)
            where TComponent1 : IComponent
            where TComponent2 : IComponent
            where TComponent3 : IComponent
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == entityPath)
            {
                throw new ArgumentNullException(nameof(entityPath));
            }

            var collector = new FilteredEntityCollector<TComponent1, TComponent2, TComponent3>();
            var observer = SubscribeInternal(entityPath.Entry, collector);
            var componentObserver = new LiveComponentObserver(observer, collector.Entities);

            observer.Subscribe(entityPath.IsAbsolute() ? entity.Root : entity);

            return componentObserver;
        }

        private LiveComponentObserver(LiveEntityCollectionWatcher watcher, IList<EntityBase> entities)
        {
            this.watcher = watcher;
            this.entities = entities;
        }

        public IEnumerator<EntityBase> GetEnumerator() => entities.GetEnumerator();

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
        /// <typeparam name="TComponent"></typeparam>
        private class FilteredEntityCollector<TComponent> : ICollectionObserver<IComponent>
            where TComponent : IComponent
        {
            public IList<EntityBase> Entities
            {
                get;
            }

            public FilteredEntityCollector()
            {
                Entities = new List<EntityBase>();
            }

            public void OnError(Exception error)
            {
                throw error;
            }

            public void OnCompleted()
            {
                ;
            }

            public void OnAdded(IComponent item)
            {
                var target = item.Entity;

                if (Entities.Contains(target))
                {
                    return;
                }

                if (FilterEntity(target))
                {
                    Entities.Add(target);
                }
            }

            public void OnRemoved(IComponent item)
            {
                var target = item.Entity;

                if (false == Entities.Contains(target))
                {
                    return;
                }

                if (false == FilterEntity(target))
                {
                    if (Entities.Remove(target))
                    {
                        ;
                    }
                }
            }

            protected virtual bool FilterEntity(EntityBase entity) => entity.Has<TComponent>();
        }

        /// <summary>
        /// 
        /// </summary>
        private class FilteredEntityCollector<TComponent1, TComponent2> : FilteredEntityCollector<TComponent1>
            where TComponent1 : IComponent
            where TComponent2 : IComponent
        {
            protected override bool FilterEntity(EntityBase entity) =>
                base.FilterEntity(entity) && entity.Has<TComponent2>();
        }

        /// <summary>
        /// 
        /// </summary>
        private class FilteredEntityCollector<TComponent1, TComponent2, TComponent3> : FilteredEntityCollector<TComponent1, TComponent2>
            where TComponent1 : IComponent
            where TComponent2 : IComponent
            where TComponent3 : IComponent
        {
            protected override bool FilterEntity(EntityBase entity) =>
                base.FilterEntity(entity) && entity.Has<TComponent3>();
        }

        /// <summary>
        /// 
        /// </summary>
        private class RootEntityCollectionWatcher : LiveEntityCollectionWatcher
        {
            private IDisposable disposable;
            private IDisposable subscription;

            public RootEntityCollectionWatcher(LiveEntityCollectionWatcher next,
                ICollectionObserver<IComponent> observer)
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