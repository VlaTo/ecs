namespace ClassLibrary1
{
    public partial class EntityFactory
    {
        private class DefaultComponentCreator : IComponentCreator
        {
            private readonly IComponentResolver resolver;

            public DefaultComponentCreator(IComponentResolver resolver)
            {
                this.resolver = resolver;
            }

            public IComponent Create(ComponentState state)
            {
                var component = resolver.Resolve(state.Alias);

                if (component is IComponentStateApply applier)
                {
                    applier.Apply(state);
                }

                return component;
            }
        }
    }
}