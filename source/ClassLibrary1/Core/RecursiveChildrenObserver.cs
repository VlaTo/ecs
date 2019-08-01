using ClassLibrary1.Extensions;
using System;
using System.Collections.Generic;
using ClassLibrary1.Core.Reactive;

namespace ClassLibrary1.Core
{
    internal class RecursiveChildrenObserver
    {
        private readonly ICollectionObserver<IComponent> next;
        private readonly Predicate<EntityBase> selector;
        private EntityBase root;
        private readonly IDictionary<EntityBase, IDisposable> subscriptions;

        public RecursiveChildrenObserver(ICollectionObserver<IComponent> next, Predicate<EntityBase> selector)
        {
            this.next = next;
            this.selector = selector;

            subscriptions = new Dictionary<EntityBase, IDisposable>();
        }

        public IDisposable Subscribe(EntityBase entity)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (ReferenceEquals(root, entity))
            {
                return subscriptions[entity];
            }

            if (null != root)
            {
                throw new InvalidOperationException();
            }

            root = entity;

            return RecursiveChildrenSubscribe(entity);
        }

        private IDisposable RecursiveChildrenSubscribe(EntityBase entity)
        {
            if (subscriptions.TryGetValue(entity, out var subscription))
            {
                return subscription;
            }

            subscription = entity.Children.Subscribe(entity, DoChildAdded, DoChildRemoved, DoChildrenCompleted);

            if (selector.Invoke(entity))
            {
                var disposable = entity.Subscribe(entity, DoComponentAdded, DoComponentRemoved);
                subscription = new CompositeDisposable(subscription, disposable);
            }

            subscriptions.Add(entity, subscription);

            return subscription;
        }

        private void UnsubscribeFromEntity(EntityBase entity)
        {
            if (subscriptions.Remove(entity, out var subscription))
            {
                /*if (disposables.Remove(entity, out var disposable))
                {
                    disposable.Dispose();
                }*/

                entity.Children.ForEach(UnsubscribeFromEntity);

                subscription.Dispose();
            }
        }

        private void DoChildAdded(EntityBase child, EntityBase entity, int index)
        {
            RecursiveChildrenSubscribe(child);
        }

        private void DoChildRemoved(EntityBase child, EntityBase entity, int index)
        {
            if (subscriptions.TryGetValue(entity, out var subscription))
            {
                if (subscription is CompositeDisposable composite)
                {
                    composite.Remove()
                }
            }
            //UnsubscribeFromEntity(child);
        }

        private void DoChildrenCompleted(EntityBase child)
        {
            UnsubscribeFromEntity(child);
        }

        private void DoComponentAdded(IComponent component, EntityBase entity, int index)
        {
            next.OnAdded(component, index);
        }

        private void DoComponentRemoved(IComponent component, EntityBase entity, int index)
        {
            next.OnRemoved(component, index);
        }
    }
}