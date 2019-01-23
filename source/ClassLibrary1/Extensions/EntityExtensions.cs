using System;
using ClassLibrary1.Core;

namespace ClassLibrary1.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="condition"></param>
        /// <param name="observer"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static IDisposable Subscribe(
            this Entity entity,
            ICondition<IComponent> condition,
            IEntityObserver observer,
            bool recursive = false)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == condition)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return SubscribeInternal(entity, condition, observer, recursive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="entity"></param>
        /// <param name="observer"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<TComponent>(
            this Entity entity,
            IEntityObserver<TComponent> observer,
            bool recursive = false)
            where TComponent : IComponent
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return SubscribeInternal(
                entity,
                new TypedComponentCondition<TComponent>(),
                new ComponentObserverProxy<TComponent>(observer),
                recursive
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="entity"></param>
        /// <param name="path"></param>
        /// <param name="observer"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<TComponent>(
            this Entity entity,
            EntityPathString path,
            IEntityObserver<TComponent> observer)
            where TComponent : IComponent
        {
            throw new NotImplementedException();
        }

        private static IDisposable SubscribeInternal(
            Entity entity,
            ICondition<IComponent> condition,
            IEntityObserver observer,
            bool recursive)
        {
            var entityObserver = new PredicateEntityObserver(condition.IsMet, observer);
            var subscription = entity.Subscribe(entityObserver);

            if (recursive)
            {
                var collectionObserver = new ChildrenCollectionObserver(entityObserver);
                subscription = entity.Children.Subscribe(collectionObserver);
            }

            return subscription;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        private class ComponentObserverProxy<TComponent> : IEntityObserver
            where TComponent : IComponent
        {
            private readonly IEntityObserver<TComponent> observer;

            public ComponentObserverProxy(IEntityObserver<TComponent> observer)
            {
                this.observer = observer;
            }

            public void OnAdded(IComponent component)
            {
                observer.OnAdded((TComponent) component);
            }

            public void OnCompleted()
            {
                observer.OnCompleted();
            }

            public void OnError(Exception error)
            {
                observer.OnError(error);
            }

            public void OnRemoved(IComponent component)
            {
                observer.OnRemoved((TComponent) component);
            }
        }
    }
}