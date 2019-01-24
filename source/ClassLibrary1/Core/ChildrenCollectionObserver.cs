using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1.Core
{
    internal class ChildrenCollectionObserver : ICollectionObserver<Entity>
    {
        private readonly IEntityObserver next;
        private readonly IDictionary<Entity, IDisposable> subscriptions;

        public ChildrenCollectionObserver(IEntityObserver next)
        {
            this.next = next;

            subscriptions = new Dictionary<Entity, IDisposable>();
        }

        public IDisposable SubscribeTo(Entity entity)
        {
            var observer = new CollectionObserver(this, entity);
            var disposable = new EntitySubscription(entity, observer, entity.Subscribe(next));

            subscriptions.Add(entity, disposable);

            return disposable;
        }

        void ICollectionObserver<Entity>.OnAdded(Entity item, int index)
        {
            SubscribeTo(item);
        }

        void ICompletable.OnCompleted()
        {
            var disposables = subscriptions.Values.ToArray();

            subscriptions.Clear();

            foreach (var subscription in disposables)
            {
                subscription.Dispose();
            }
        }

        void IError.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        void ICollectionObserver<Entity>.OnRemoved(Entity item, int index)
        {
            if (subscriptions.Remove(item, out var disposable))
            {
                disposable.Dispose();
            }
        }

        private void DoAdded(CollectionObserver observer, Entity entity)
        {
            SubscribeTo(entity);
        }

        private void DoCompleted(CollectionObserver observer)
        {
            ;
        }

        private void DoError(CollectionObserver observer, Exception exception)
        {
            ;
        }

        private void DoRemoved(CollectionObserver observer, Entity entity)
        {
            if (subscriptions.Remove(entity, out var disposable))
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class CollectionObserver : ICollectionObserver<Entity>
        {
            private readonly ChildrenCollectionObserver parent;

            public Entity Entity
            {
                get;
            }

            public CollectionObserver(ChildrenCollectionObserver parent, Entity entity)
            {
                this.parent = parent;
                Entity = entity;
            }

            void ICollectionObserver<Entity>.OnAdded(Entity item, int index)
            {
                parent.DoAdded(this, item);
            }

            void ICompletable.OnCompleted()
            {
                parent.DoCompleted(this);
            }

            void IError.OnError(Exception error)
            {
                parent.DoError(this, error);
            }

            void ICollectionObserver<Entity>.OnRemoved(Entity item, int index)
            {
                parent.DoRemoved(this, item);
            }

            /*private void ReleaseSubscriptions()
            {
                var disposables = subscriptions.Values.ToArray();

                subscriptions.Clear();

                foreach (var subscription in disposables)
                {
                    subscription.Dispose();
                }
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        private class EntitySubscription : IDisposable
        {
            private readonly Entity entity;
            private readonly IDisposable disposable;
            private readonly IDisposable subscription;

            public EntitySubscription(
                Entity entity,
                CollectionObserver observer,
                IDisposable disposable
            )
            {
                this.entity = entity;
                this.disposable = disposable;
                subscription = entity.Children.Subscribe(observer);
            }

            void IDisposable.Dispose()
            {
                disposable.Dispose();
                subscription.Dispose();
            }
        }
    }
}