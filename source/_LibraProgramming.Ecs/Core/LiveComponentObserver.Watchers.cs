using LibraProgramming.Ecs.Core.Reactive;
using System;
using System.Collections.Generic;

namespace LibraProgramming.Ecs.Core
{
    public partial class LiveComponentObserver
    {
        /// <summary>
        /// 
        /// </summary>
        private class RootEntityCollectionWatcher : LiveEntityCollectionWatcher
        {
            private readonly Dictionary<EntityBase, IDisposable> entities;
            private RefCountDisposable subscription;
            //private IDisposable subscription;

            public RootEntityCollectionWatcher(LiveEntityCollectionWatcher next,
                IScopedCollectionObserver<IComponent, EntityBase> observer)
                : base(next, observer)
            {
                entities = new Dictionary<EntityBase, IDisposable>();
                subscription = new RefCountDisposable(Disposable.Empty);
            }

            public override IDisposable Subscribe(EntityBase entity)
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

                var disposable = null != Next
                    ? entity.Children.Subscribe(this)
                    : entity.Subscribe(Observer);

                subscription = new RefCountDisposable(disposable);

                return subscription.GetDisposable();
            }

            protected override void DoAdded(EntityBase entity)
            {
                EnsureNotDisposed();

                if (false == entities.ContainsKey(entity))
                {
                    entities.Add(entity, Next.Subscribe(entity));
                }
            }

            protected override void DoRemoved(EntityBase entity)
            {
                EnsureNotDisposed();

                if (entities.Remove(entity, out var disposable))
                {
                    disposable.Dispose();
                }
            }

            protected override void OnDispose()
            {
                foreach (var kvp in entities)
                {
                    ;
                }

                subscription.Dispose();

                base.OnDispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class KeyEntityCollectionWatcher : LiveEntityCollectionWatcher
        {
            private readonly Dictionary<EntityBase, IDisposable> entities;
            private readonly Dictionary<EntityBase, RefCountDisposable> subscriptions;

            public string EntityKey
            {
                get;
            }

            public KeyEntityCollectionWatcher(
                LiveEntityCollectionWatcher next,
                IScopedCollectionObserver<IComponent, EntityBase> observer,
                string entityKey)
                : base(next, observer)
            {
                entities = new Dictionary<EntityBase, IDisposable>();
                subscriptions = new Dictionary<EntityBase, RefCountDisposable>();

                EntityKey = entityKey;
            }

            public override IDisposable Subscribe(EntityBase entity)
            {
                EnsureNotDisposed();

                if (null == entity)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                if (false == String.Equals(entity.Key, EntityKey))
                {
                    return Disposable.Empty;
                }

                if (false == subscriptions.TryGetValue(entity, out var subscription))
                {
                    var disposable = new CompositeDisposable(
                        entity.Subscribe(Observer.CreateScope(entity)),
                        Disposable.CreateWithState(entity, DoUnsubscribe)
                    );

                    if (null != Next)
                    {
                        disposable.Add(entity.Children.Subscribe(this));
                    }

                    subscription = new RefCountDisposable(disposable);

                    subscriptions.Add(entity, subscription);
                }

                return subscription.GetDisposable();
            }

            protected override void DoAdded(EntityBase entity)
            {
                EnsureNotDisposed();

                if (false == entities.ContainsKey(entity))
                {
                    entities.Add(entity, Next.Subscribe(entity));
                }
            }

            protected override void DoRemoved(EntityBase entity)
            {
                EnsureNotDisposed();

                if (entities.Remove(entity, out var disposable))
                {
                    disposable.Dispose();
                }
            }

            protected override void OnDispose()
            {
                foreach (var entity in entities)
                {
                    ;
                }

                foreach (var kvp in subscriptions)
                {
                    kvp.Value.Dispose();
                }

                base.OnDispose();
            }

            private void DoUnsubscribe(EntityBase entity)
            {
                EnsureNotDisposed();

                if (subscriptions.Remove(entity, out var subscription))
                {
                    ;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class WildcardEntityCollectionWatcher : LiveEntityCollectionWatcher
        {
            private readonly Dictionary<EntityBase, IDisposable> entities;
            private readonly Dictionary<EntityBase, RefCountDisposable> subscriptions;

            public WildcardEntityCollectionWatcher(
                LiveEntityCollectionWatcher next,
                IScopedCollectionObserver<IComponent, EntityBase> observer)
                : base(next, observer)
            {
                entities = new Dictionary<EntityBase, IDisposable>();
                subscriptions = new Dictionary<EntityBase, RefCountDisposable>();
            }

            public override IDisposable Subscribe(EntityBase entity)
            {
                EnsureNotDisposed();

                if (null == entity)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                if (false == subscriptions.TryGetValue(entity, out var subscription))
                {
                    var disposable = new CompositeDisposable(
                        entity.Subscribe(Observer.CreateScope(entity)),
                        Disposable.CreateWithState(entity, DoUnsubscribe)
                    );

                    if (null != Next)
                    {
                        disposable.Add(entity.Children.Subscribe(this));
                    }

                    subscription = new RefCountDisposable(disposable);

                    subscriptions.Add(entity, subscription);
                }

                return subscription.GetDisposable();
            }

            protected override void DoAdded(EntityBase entity)
            {
                EnsureNotDisposed();

                if (false == entities.ContainsKey(entity))
                {
                    entities.Add(entity, Next.Subscribe(entity));
                }
            }

            protected override void DoRemoved(EntityBase entity)
            {
                EnsureNotDisposed();

                if (entities.Remove(entity, out var disposable))
                {
                    disposable.Dispose();
                }
            }

            protected override void OnDispose()
            {
                foreach (var entity in entities)
                {
                    ;
                }

                foreach (var kvp in subscriptions)
                {
                    kvp.Value.Dispose();
                }

                base.OnDispose();
            }

            private void DoUnsubscribe(EntityBase entity)
            {
                EnsureNotDisposed();

                if (subscriptions.Remove(entity, out var subscription))
                {
                    ;
                }
            }
        }
    }
}