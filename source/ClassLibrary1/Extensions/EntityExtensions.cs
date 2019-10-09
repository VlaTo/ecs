using System;
using System.Collections.Generic;
using System.ComponentModel;
using ClassLibrary1.Core;
using ClassLibrary1.Core.Extensions;
using ClassLibrary1.Core.Path;
using ClassLibrary1.Core.Reactive.Collections;

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
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="entity"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static TComponent Add<TComponent>(this EntityBase entity, Action<TComponent> initializer = null)
            where TComponent : IComponent, new()
        {
            var component = new TComponent();

            initializer?.Invoke(component);

            entity.Add(component);

            return component;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<TComponent> GetAllRecursive<TComponent>(this EntityBase entity)
            where TComponent : class, IComponent
        {
            var collection = new List<TComponent>();

            while (null != entity)
            {
                collection.AddRange(entity.GetAll<TComponent>());
                entity = entity.Parent;
            }

            return collection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="condition"></param>
        /// <param name="observer"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static IDisposable Subscribe(
            this EntityBase entity,
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

            /*var entityObserver = new PredicateEntityObserver(condition.IsMet, observer);

            if (recursive)
            {
                var collectionObserver = new ChildrenCollectionObserver(entityObserver, Condition<EntityBase>.True);
                return collectionObserver.SubscribeTo(entity);
            }

            return entity.Subscribe(entityObserver);*/

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="onAdded"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        /*public static IDisposable Subscribe(
            this EntityBase entity,
            Action<IComponent> onAdded,
            Action onCompleted)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == onAdded)
            {
                throw new ArgumentNullException(nameof(onAdded));
            }

            return entity.Subscribe(CollectionObserver.Create(onAdded, onCompleted));
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="observer"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static IDisposable Subscribe(
            this EntityBase entity,
            ICollectionObserver<IComponent> observer,
            bool recursive = false)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            if (recursive)
            {
                var collectionObserver = new RecursiveChildrenObserver(observer, Condition<EntityBase>.True);
                return collectionObserver.Subscribe(entity);
            }

            return entity.Subscribe(observer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="path"></param>
        /// <param name="observer"></param>
        /// <returns></returns>
        public static IDisposable Subscribe(
            this EntityBase entity,
            EntityPath path,
            ICollectionObserver<IComponent> observer)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var pathMatch = new EntityPathMatch(path);
            var collectionObserver = new RecursiveChildrenObserver(observer, pathMatch.IsMet);

            return collectionObserver.Subscribe(path.IsRelative() ? entity : entity.Root);

            /*var match = new EntityPathMatch(path);
            var entityObserver = new ComponentEntityObserver(observer);
            var collectionObserver = new RecursiveChildrenObserver(entityObserver, match.IsMet);

            return collectionObserver.SubscribeTo(entity);*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="entity"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static LiveComponentObserver<TComponent> Subscribe<TComponent>(this EntityBase entity, EntityPath path)
            where TComponent : IComponent
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return LiveComponentObserver<TComponent>.Subscribe(entity, path);
        }
    }
}