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

            var entityObserver = new PredicateEntityObserver(condition.IsMet, observer);

            if (recursive)
            {
                var collectionObserver = new ChildrenCollectionObserver(entityObserver);
                return collectionObserver.SubscribeTo(entity);
            }

            return entity.Subscribe(entityObserver);
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

            var entityObserver = new TypedComponentEntityObserver<TComponent>(observer);

            if (recursive)
            {
                var collectionObserver = new ChildrenCollectionObserver(entityObserver);
                return collectionObserver.SubscribeTo(entity);
            }

            return entity.Subscribe(entityObserver);
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
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var match = new EntityPathMatch(path, entity);
            var entityObserver = new PredicateEntityObserver<TComponent>(match.IsMet, observer);
            var collectionObserver = new ChildrenCollectionObserver(entityObserver);

            return collectionObserver.SubscribeTo(entity);
        }
    }
}