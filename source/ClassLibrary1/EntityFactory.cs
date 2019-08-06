using System;
using ClassLibrary1.Core.Path;

namespace ClassLibrary1
{
    public partial class EntityFactory
    {
        private readonly IEntityCreator entityCreator;

        public static readonly EntityFactory Default;

        public IPrototypeResolver PrototypeResolver
        {
            get;
        }

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
            var entityCreator = new DefaultEntityCreator(componentCreator);
            var prototypeResolver = new DefaultPrototypeResolver();

            Default = new EntityFactory(entityCreator, prototypeResolver, componentRegistry);
        }

        public EntityBase CreateEntity(string key, EntityPath prototypePath)
        {
            var prototype = PrototypeResolver.Resolve(prototypePath);
            return CreateEntity(key, prototype);
        }

        public EntityBase CreateEntity(EntityState state)
        {
            return entityCreator.Instantiate(state);
            var instance = new Entity(state.Key);

            foreach (var componentState in state.Components)
            {
                var child = Com CreateEntity(childState);
                instance.Children.Add(child);
            }

            foreach (var childState in state.Children)
            {
                var child = CreateEntity(childState);
                instance.Children.Add(child);
            }

            return instance;
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