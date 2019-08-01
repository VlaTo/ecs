using System;

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

        public EntityBase CreateEntity(EntityPathString prototypePath)
        {
            var prototype = PrototypeResolver.Resolve(prototypePath);
            var state = new EntityState();
            return CreateEntity(state, prototype);
        }

        public EntityBase CreateEntity(EntityState state)
        {
            return CreateEntity(state, null);
        }

        public EntityBase CreateEntity(EntityState state, EntityBase prototype)
        {
            if (null == state)
            {
                throw new ArgumentNullException(nameof(state));
            }

            var entity = entityCreator.Instantiate(state);

            foreach (var childState in state.Children)
            {
                var child = CreateEntity(childState);
                entity.Children.Add(child);
            }

            return entity;
        }
    }
}