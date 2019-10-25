using System;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prototypePath"></param>
        /// <returns></returns>
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

        public void LoadEntity(EntityBase entity, EntityState state)
        {
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
        }

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

        private EntityBase InstantiateEntity(EntityState entityState)
        {
            if (false == String.IsNullOrEmpty(entityState.EntityPath))
            {
                if (entityState.IsReference)
                {
                    return new ReferencedEntity(entityState.Key, entityState.EntityPath);
                }
                
                //return new Entity(entityState.Key, entityState.EntityPath);
                throw new NotSupportedException();
            }

            return new Entity(entityState.Key);
        }
    }
}