using LibraProgramming.Ecs;

namespace ConsoleApp2.Components
{
    [Component(Alias = nameof(RenderComponent))]
    public sealed class RenderComponent : Component
    {
        public RenderComponent()
        {
        }

        private RenderComponent(RenderComponent instance)
        {
            ;
        }

        public override IComponent Clone()
        {
            return new RenderComponent(this);
        }

        protected override void DoAttach()
        {
            ;
        }

        protected override void DoRelease()
        {
            ;
        }
    }
}