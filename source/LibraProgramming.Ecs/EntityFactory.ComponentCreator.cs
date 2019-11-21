namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EntityLoader
    {
        private class DefaultComponentCreator : IComponentCreator
        {
            private readonly IComponentResolver resolver;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="resolver"></param>
            public DefaultComponentCreator(IComponentResolver resolver)
            {
                this.resolver = resolver;
            }

            /// <inheritdoc cref="IComponentCreator.Create" />
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