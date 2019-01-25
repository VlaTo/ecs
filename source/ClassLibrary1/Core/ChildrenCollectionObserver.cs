using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1.Core
{
    internal class ChildrenCollectionObserver : ICollectionObserver<Entity>
    {
        private readonly IEntityObserver next;
        private readonly Predicate<Entity> selector;
        private readonly IDictionary<Entity, IDisposable> subscriptions;

        public ChildrenCollectionObserver(IEntityObserver next, Predicate<Entity> selector)
        {
            this.next = next;
            this.selector = selector;

            subscriptions = new Dictionary<Entity, IDisposable>();
        }

        public IDisposable SubscribeTo(Entity entity)
        {
            SubscribeToEntity(entity);

            return subscriptions[entity];
        }

        void ICollectionObserver<Entity>.OnAdded(Entity item, int index)
        {
            SubscribeToEntity(item);
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

        private void SubscribeToEntity(Entity entity)
        {
            var observer = new CollectionObserver(this, entity);
            var disposable = new EntitySubscription(this, entity, observer);

            subscriptions.Add(entity, disposable);
        }

        private void DoAdded(CollectionObserver observer, Entity entity)
        {
            SubscribeToEntity(entity);
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
            private readonly ChildrenCollectionObserver owner;
            private readonly IDisposable entitySubscription;
            private readonly IDisposable childSubscription;

            public EntitySubscription(
                ChildrenCollectionObserver owner,
                Entity entity,
                CollectionObserver observer
            )
            {
                this.owner = owner;

                if (owner.selector.Invoke(entity))
                {
                    entitySubscription = entity.Subscribe(owner.next);
                }

                childSubscription = entity.Children.Subscribe(observer);
            }

            void IDisposable.Dispose()
            {
                entitySubscription?.Dispose();
                childSubscription.Dispose();
            }
        }
    }
}