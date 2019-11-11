using LibraProgramming.Ecs.Core.Extensions;
using LibraProgramming.Ecs.Core.Path;
using LibraProgramming.Ecs.Core.Path.Extensions;
using LibraProgramming.Ecs.Core.Path.Segments;
using LibraProgramming.Ecs.Core.Reactive.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LibraProgramming.Ecs.Core
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LiveComponentObserver : IEnumerable<EntityBase>, IDisposable
    {
        private LiveEntityCollectionWatcher watcher;
        private IList<EntityBase> entities;
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

            var collector = new FilteredEntityCollector<TComponent>();
            var watcher = CreateWatcher(entityPath.Entry, collector);
            var componentObserver = new LiveComponentObserver(watcher, collector.Entities);

            watcher.Subscribe(entityPath.IsAbsolute() ? entity.Root : entity);

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

            var collector = new FilteredEntityCollector<TComponent1, TComponent2>();
            var watcher = CreateWatcher(entityPath.Entry, collector);
            var componentObserver = new LiveComponentObserver(watcher, collector.Entities);

            watcher.Subscribe(entityPath.IsAbsolute() ? entity.Root : entity);

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
            var watcher = CreateWatcher(entityPath.Entry, collector);
            var componentObserver = new LiveComponentObserver(watcher, collector.Entities);

            watcher.Subscribe(entityPath.IsAbsolute() ? entity.Root : entity);

            return componentObserver;
        }

        private LiveComponentObserver(LiveEntityCollectionWatcher watcher, IList<EntityBase> entities)
        {
            this.watcher = watcher;
            this.entities = entities;
        }

        public IEnumerator<EntityBase> GetEnumerator() => new MutualEntityEnumerator(entities);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            Dispose(true);
        }

        private static LiveEntityCollectionWatcher CreateWatcher(EntityPathSegment path, IScopedCollectionObserver<IComponent, EntityBase> observer)
        {
            if (null == path)
            {
                return null;
            }

            var next = CreateWatcher(path.Next, observer);

            if (path.IsRoot())
            {
                return new RootEntityCollectionWatcher(next, observer);
            }

            if (path.IsEntityKey(out var key))
            {
                return new KeyEntityCollectionWatcher(next, observer, key);
            }

            if (path.IsAnyKey())
            {
                return new WildcardEntityCollectionWatcher(next, observer);
            }

            if (path.IsAnyPathLevel())
            {
                return new AnyPathLevelEntityCollectionWatcher(next, observer);
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
                    watcher = null;
                    entities = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}