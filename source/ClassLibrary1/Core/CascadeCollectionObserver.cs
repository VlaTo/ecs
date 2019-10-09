using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using ClassLibrary1.Core.Reactive.Collections;

namespace ClassLibrary1.Core
{
    internal sealed class CascadeCollectionObserver : IDisposable
    {
        private readonly List<EntityBase> entities;

        public CascadeCollectionObserver(CascadeCollectionObserver parent)
        {
            ;
        }

        /*public static CascadeCollectionObserver Create(CascadeCollectionObserver parent, EntityBase entity, string entityKey)
        {
            if (null == parent)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }


            //var observer = CollectionObserver.Create<IComponent>(OnAdded, OnRemoved, OnCompleted);
            var observer = new CascadeCollectionObserver(parent, entity);
            var index = entity.Children.FindIndex(item => item.Key == entityKey);
            IDisposable disposable;

            if (0 <= index)
            {
                var child = entity.Children[index];
                disposable = child.Subscribe(observer);
            }
            else
            {
                var entityObserver = new DeferredEntityObserver(entityKey, observer);
                disposable = entity.Children.Subscribe(entityObserver);
            }

            return new CascadeCollectionObserver(parent, entity, observer, disposable);
        }*/

        public void Subscribe(EntityBase entity, string childEntityKey)
        {
            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == childEntityKey)
            {
                throw new ArgumentNullException(nameof(childEntityKey));
            }

            if (0 == childEntityKey.Length)
            {
                throw new ArgumentException("", nameof(childEntityKey));
            }

            //var observer = CollectionObserver.Create<EntityBase, EntityBase>(entity, OnChildAdded, OnChildRemoved, OnObserverCompleted);
            var observer = new EntityCollectionObserver(this, entity, childEntityKey);
            var disposable = entity.Children.Subscribe(observer);

            //entities.Add(entity, disposable);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        private class EntityCollectionObserver : ICollectionObserver<EntityBase>
        {
            private readonly CascadeCollectionObserver observer;
            private readonly EntityBase parent;
            private readonly string childEntityKey;

            public EntityCollectionObserver(CascadeCollectionObserver observer, EntityBase parent, string childEntityKey)
            {
                this.observer = observer;
                this.parent = parent;
                this.childEntityKey = childEntityKey;
            }

            public void OnCompleted()
            {
                ;
            }

            public void OnError(Exception error)
            {
                ;
            }

            public void OnAdded(EntityBase child)
            {
                if (false == String.Equals(child.Key, childEntityKey))
                {
                    return;
                }


            }

            public void OnRemoved(EntityBase child)
            {
                if (false == String.Equals(child.Key, childEntityKey))
                {
                    return;
                }


            }
        }
    }
}