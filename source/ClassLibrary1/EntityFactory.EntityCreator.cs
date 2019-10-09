namespace ClassLibrary1
{
    public partial class EntityFactory
    {
        private class DefaultEntityCreator : IEntityCreator
        {
            private readonly IComponentCreator componentCreator;
            private readonly IPrototypeResolver prototypeResolver;

            public DefaultEntityCreator(IComponentCreator componentCreator, IPrototypeResolver prototypeResolver)
            {
                this.componentCreator = componentCreator;
                this.prototypeResolver = prototypeResolver;
            }

            public EntityBase Instantiate(EntityState state)
            {
                var instance = new Entity(state.Key);

                foreach (var componentState in state.Components)
                {
                    var component = componentCreator.Create(componentState);
                    instance.Add(component);
                }

                foreach (var entityState in state.Children)
                {
                    var child = Instantiate(entityState);
                    instance.Children.Add(child);
                }
                
                return instance;
            }
        }
    }
}