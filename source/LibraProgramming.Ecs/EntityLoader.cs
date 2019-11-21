using System;
using System.Collections.Generic;

namespace LibraProgramming.Ecs
{
    public enum EntityLoadStage
    {
        NotLoaded = -1,
        Loading,
        LateBinding
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EntityLoader
    {
        public EntityLoadStage LoadStage
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IComponentResolver ComponentResolver
        {
            get;
        }

        public EntityLoader(IComponentResolver componentResolver)
        {
            if (null == componentResolver)
            {
                throw new ArgumentNullException(nameof(componentResolver));
            }

            ComponentResolver = componentResolver;
            LoadStage = EntityLoadStage.NotLoaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public void LoadEntity(EntityBase entity, EntityState state)
        {
            var deferred = new Queue<(EntityBase, EntityState)>();

            LoadOrDeferEntity(entity, state, deferred);
            ResolveDeferred(deferred);
        }

        private void LoadOrDeferEntity(EntityBase entity, EntityState state, Queue<(EntityBase, EntityState)> deferred)
        {
            LoadStage = EntityLoadStage.Loading;

            foreach (var childState in state.Children)
            {
                LoadEntityCore(entity, childState, deferred);
            }
        }

        private void ResolveDeferred(Queue<(EntityBase, EntityState)> deferred)
        {
            LoadStage = EntityLoadStage.LateBinding;

            while (0 < deferred.Count)
            {
                var (parent, state) = deferred.Dequeue();

                LoadLinkedEntity(parent, state, deferred);
            }
        }

        private void LoadEntityCore(EntityBase parent, EntityState state, Queue<(EntityBase, EntityState)> deferred)
        {
            if (false == String.IsNullOrEmpty(state.EntityPath))
            {
                deferred.Enqueue((parent, state));

                /*if (false == deferred.TryGetValue(parent, out var children))
                {
                    children = new List<EntityState>();
                    deferred.Add(parent, children);
                }

                children.Add(state);*/

                return;
            }

            var entity = new Entity(state.Key);

            foreach (var componentState in state.Components)
            {
                var component = (Component) ComponentResolver.Resolve(componentState.Alias);

                component.ApplyState(componentState);
                entity.Add(component);
            }

            foreach (var childState in state.Children)
            {
                LoadEntityCore(entity, childState, deferred);
            }

            parent.Children.Add(entity);
        }

        private static void LoadLinkedEntity(EntityBase parent, EntityState state, Queue<(EntityBase, EntityState)> deferred)
        {
            if (String.IsNullOrEmpty(state.EntityPath))
            {
                throw new EntityException();
            }

            var prototype = parent.Find(state.EntityPath);

            if (state.IsReference)
            {
                if (null == prototype)
                {
                    deferred.Enqueue((parent, state));
                }
                else
                {
                    parent.Children.Add(new LinkedEntity(state.Key, prototype));
                }

                return;
            }

            if (null == prototype)
            {
                throw new EntityException();
            }

            var entity = new Entity(state.Key);
            
            foreach (var component in prototype.Components)
            {
                entity.Add(component.Clone());
            }

            foreach (var child in prototype.Children)
            {
                entity.Children.Add(child.Clone());
            }

            parent.Children.Add(entity);
        }
    }
}