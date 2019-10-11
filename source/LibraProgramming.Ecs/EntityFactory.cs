using System;
using LibraProgramming.Ecs.Core.Path;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EntityFactory
    {
        private readonly IEntityCreator entityCreator;

        /// <summary>
        /// 
        /// </summary>
        public static readonly LibraProgramming.Ecs.EntityFactory Default;

        /// <summary>
        /// 
        /// </summary>
        public IPrototypeResolver PrototypeResolver
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public IComponentRegistry ComponentRegistry
        {
            get;
        }

        private EntityFactory(IEntityCreator entityCreator, IPrototypeResolver prototypeResolver, IComponentRegistry componentRegistry)
        {
            if (null == entityCreator)
            {
                throw new ArgumentNullException(nameof(entityCreator));
            }

            this.entityCreator = entityCreator;
            PrototypeResolver = prototypeResolver;
            ComponentRegistry = componentRegistry;
        }

        static EntityFactory()
        {
            var componentRegistry = new DefaultComponentRegistry();
            var componentCreator = new DefaultComponentCreator(componentRegistry);
            var prototypeResolver = new DefaultPrototypeResolver();
            var entityCreator = new DefaultEntityCreator(componentCreator, prototypeResolver);

            Default = new LibraProgramming.Ecs.EntityFactory(entityCreator, prototypeResolver, componentRegistry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prototypePath"></param>
        /// <returns></returns>
        public EntityBase CreateEntity(string key, EntityPath prototypePath)
        {
            if (null == prototypePath)
            {
                var instance = new Entity(key);
                return instance;
            }

            var prototype = PrototypeResolver.Resolve(prototypePath);

            return CreateEntity(key, prototype);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public EntityBase CreateEntity(EntityState state)
        {
            return entityCreator.Instantiate(state);

            /*var instance = new Entity(state.Key);

            foreach (var componentState in state.Components)
            {
                var child = CreateEntity(childState);
                instance.Children.Add(child);
            }

            foreach (var childState in state.Children)
            {
                var child = CreateEntity(childState);
                instance.Children.Add(child);
            }

            return instance;*/
        }

        private EntityBase CreateEntity(string key, EntityBase prototype)
        {
            if (null == prototype)
            {
                throw new ArgumentNullException(nameof(prototype));
            }

            var instance = new Entity(key, prototype);

            return instance;
        }
    }
}