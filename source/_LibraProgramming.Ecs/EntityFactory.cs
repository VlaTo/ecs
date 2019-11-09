using System;
using System.Collections.Generic;
using System.Reflection;
using LibraProgramming.Ecs.Core;
using LibraProgramming.Ecs.Core.Path;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EntityFactory
    {
        //private readonly IEntityCreator entityCreator;

        /// <summary>
        /// 
        /// </summary>
        /*public IPrototypeResolver PrototypeResolver
        {
            get;
        }*/

        /// <summary>
        /// 
        /// </summary>
        public IComponentResolver ComponentResolver
        {
            get;
        }

        public EntityFactory(
            /*IEntityCreator entityCreator,
            IPrototypeResolver prototypeResolver,*/
            IComponentResolver componentResolver)
        {
            if (null == componentResolver)
            {
                throw new ArgumentNullException(nameof(componentResolver));
            }

            //this.entityCreator = entityCreator;
            //PrototypeResolver = prototypeResolver;
            ComponentResolver = componentResolver;
        }

        /*public EntityBase CreateEntity(string key, EntityPath prototypePath)
        {
            if (null == prototypePath)
            {
                var instance = new Entity(key);
                return instance;
            }

            var prototype = PrototypeResolver.Resolve(prototypePath);

            return CreateEntity(key, prototype);
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public void LoadEntity(EntityBase entity, EntityState state)
        {
            var deferred = new Dictionary<EntityBase, IList<EntityState>>();

            foreach (var childState in state.Children)
            {
                LoadEntityCore(entity, childState, deferred);
            }

            foreach (var (parent, states) in deferred)
            {
                foreach (var childState in states)
                {
                    LoadLinkedEntity(parent, childState);
                }
            }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public EntityBase CreateEntity(EntityState state)
        {
            var entity = InstantiateEntity(state);

            foreach (var componentState in state.Components)
            {
                var component = CreateComponent(componentState);
                entity.Add(component);
            }

            foreach (var childState in state.Children)
            {
                var child = CreateEntity(childState);
                entity.Children.Add(child);
            }

            return entity;
        }*/

        private void LoadEntityCore(EntityBase parent, EntityState state,
            IDictionary<EntityBase, IList<EntityState>> deferred)
        {
            if (false == String.IsNullOrEmpty(state.EntityPath))
            {
                if (false == deferred.TryGetValue(parent, out var children))
                {
                    children = new List<EntityState>();
                    deferred.Add(parent, children);
                }

                children.Add(state);

                return;
            }

            var entity = new Entity(state.Key);

            LoadComponents(entity, state.Components);

            foreach (var childState in state.Children)
            {
                LoadEntityCore(entity, childState, deferred);
            }

            parent.Children.Add(entity);
        }

        private void LoadLinkedEntity(EntityBase parent, EntityState state)
        {
            if (String.IsNullOrEmpty(state.EntityPath))
            {
                throw new EntityException();
            }

            if (state.IsReference)
            {
                parent.Children.Add(new LinkedEntity(state.Key, state.EntityPath));
                return;
            }

            var prototype = parent.Find(state.EntityPath);

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

            /*LoadComponents(entity, state.Components);

            foreach (var childState in state.Children)
            {
                LoadLinkedEntity(entity, childState);
            }*/

            parent.Children.Add(entity);
        }


        /*private EntityBase CreateEntity(string key, EntityBase prototype)
        {
            if (null == prototype)
            {
                throw new ArgumentNullException(nameof(prototype));
            }

            var instance = new Entity(key, prototype);

            return instance;
        }*/

        private IComponent CreateComponent(ComponentState state)
        {
            var component = (Component) ComponentResolver.Resolve(state.Alias);
            
            component.ApplyState(state);
            
            return component;
        }

        private void LoadComponents(EntityBase entity, ComponentState[] componentStates)
        {
            foreach (var state in componentStates)
            {
                var component = CreateComponent(state);
                entity.Add(component);
            }
        }

        /*private EntityBase InstantiateEntity(EntityState entityState)
        {
            if (false == String.IsNullOrEmpty(entityState.EntityPath))
            {
                if (entityState.IsReference)
                {
                    return new LinkedEntity(entityState.Key, entityState.EntityPath);
                }
                
                return new Entity(entityState.Key, entityState);
            }

            return new Entity(entityState.Key);
        }*/
    }
}