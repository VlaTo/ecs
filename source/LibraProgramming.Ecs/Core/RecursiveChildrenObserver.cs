﻿using LibraProgramming.Ecs.Core.Reactive;
using LibraProgramming.Ecs.Core.Reactive.Collections;
using LibraProgramming.Ecs.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using LibraProgramming.Ecs.Core.Extensions;

namespace LibraProgramming.Ecs.Core
{
    internal class RecursiveChildrenObserver
    {
        private readonly Predicate<EntityBase> selector;
        private readonly IDictionary<EntityBase, IDisposable> subscriptions;
        private readonly RefCountCollectionObserver<IComponent> subscriber;
        private EntityBase root;

        public RecursiveChildrenObserver(ICollectionObserver<IComponent> next, Predicate<EntityBase> selector)
        {
            this.selector = selector;

            subscriptions = new Dictionary<EntityBase, IDisposable>();
            subscriber = new RefCountCollectionObserver<IComponent>(next);
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
            Debug.WriteLine($"RecursiveChildrenSubscribe: \'{entity.Path}\'");
            if (subscriptions.TryGetValue(entity, out var subscription))
            {
                return subscription;
            }

            subscriptions.Add(entity, Disposable.Empty);

            subscription = entity.Children.Subscribe(entity, DoChildAdded, DoChildRemoved, DoChildrenCompleted);

            if (selector.Invoke(entity))
            {
                var disposable = entity.Subscribe(subscriber.GetObserver());
                subscription = new CompositeDisposable(subscription, disposable);
            }

            AddEntitySubscription(entity, subscription);

            if (null != entity.Parent)
            {
                AddEntitySubscription(entity.Parent, subscription);
            }

            return subscription;
        }

        private void AddEntitySubscription(EntityBase entity, IDisposable subscription)
        {
            if (false == subscriptions.TryGetValue(entity, out var disposable))
            {
                throw new Exception();
            }

            if (disposable is CompositeDisposable composite)
            {
                composite.Add(subscription);
                return;
            }

            subscriptions[entity] = ReferenceEquals(Disposable.Empty, disposable)
                ? subscription
                : new CompositeDisposable(disposable, subscription);
        }

        private void DoChildAdded(EntityBase child, EntityBase parent)
        {
            Debug.WriteLine($"Child added: \'{child.Path}\'");
            //child.Parent
            RecursiveChildrenSubscribe(child);
        }

        private void DoChildRemoved(EntityBase child, EntityBase parent)
        {
            Debug.WriteLine($"Child removing: \'{child.Path}\'");
            //child.Parent
            if (subscriptions.Remove(child, out var subscription))
            {
                if (subscription is CompositeDisposable composite)
                {
                    //composite.Dispose()
                }

                subscription.Dispose();
            }
        }

        private void DoChildrenCompleted(EntityBase owner)
        {
            Debug.WriteLine($"Owner completed: \'{owner.Path}\'");
            if (subscriptions.Remove(owner, out var subscription))
            {
                if (subscription is CompositeDisposable composite)
                {
                    //composite.Dispose()
                }

                subscription.Dispose();
            }
        }
    }
}