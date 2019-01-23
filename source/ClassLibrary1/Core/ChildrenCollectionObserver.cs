using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1.Core
{
    internal class ChildrenCollectionObserver : ICollectionObserver<Entity>
    {
        private readonly IEntityObserver next;
        private readonly IDictionary<Observer, IDisposable> tokens;
        private readonly IDictionary<Entity, IDisposable> subscriptions;

        public ChildrenCollectionObserver(IEntityObserver next)
        {
            this.next = next;

            tokens = new Dictionary<Observer, IDisposable>();
            subscriptions = new Dictionary<Entity, IDisposable>();
        }

        public void OnAdded(Entity item, int index)
        {
            var collectionObserver = new Observer(this);
            var subscription = item.Children.Subscribe(collectionObserver);

            subscriptions.Add(item, subscription);
            tokens.Add(collectionObserver, item.Subscribe(next));
        }

        public void OnCompleted()
        {
            var disposables = subscriptions.Values.ToArray();

            subscriptions.Clear();

            foreach (var subscription in disposables)
            {
                subscription.Dispose();
            }
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnRemoved(Entity item, int index)
        {
            if (subscriptions.Remove(item, out var subscription))
            {
                subscription.Dispose();
            }
        }

        private void DoObserverAdded(Observer observer, Entity entity)
        {
            tokens.Add(observer, entity.Subscribe(next));
        }

        private void DoObserverCompleted(Observer observer)
        {
            if (tokens.Remove(observer, out var subscription))
            {
                subscription.Dispose();
            }
        }

        private void DoObserverError(Observer o, Exception exception)
        {

        }

        private void DoObserverRemoved(Observer observer, Entity entity)
        {
            if (tokens.Remove(observer, out var subscription))
            {
                subscription.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Observer : ICollectionObserver<Entity>
        {
            private readonly ChildrenCollectionObserver parent;
            private readonly IDictionary<Entity, IDisposable> subscriptions;

            public Observer(ChildrenCollectionObserver parent)
            {
                this.parent = parent;
                subscriptions = new Dictionary<Entity, IDisposable>();
            }

            public void OnAdded(Entity item, int index)
            {
                parent.DoObserverAdded(this, item);

                var collectionObserver = new Observer(parent);
                var subscription = item.Children.Subscribe(collectionObserver);

                subscriptions.Add(item, subscription);
            }

            public void OnCompleted()
            {
                parent.DoObserverCompleted(this);

                ReleaseSubscriptions();
            }

            public void OnError(Exception error)
            {
                parent.DoObserverError(this, error);

                ReleaseSubscriptions();
            }

            public void OnRemoved(Entity item, int index)
            {
                parent.DoObserverRemoved(this, item);

                if (subscriptions.Remove(item, out var subscription))
                {
                    subscription.Dispose();
                }
            }

            private void ReleaseSubscriptions()
            {
                var disposables = subscriptions.Values.ToArray();

                subscriptions.Clear();

                foreach (var subscription in disposables)
                {
                    subscription.Dispose();
                }
            }
        }
    }
}