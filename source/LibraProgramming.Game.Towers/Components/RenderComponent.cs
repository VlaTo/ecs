using LibraProgramming.Ecs;

namespace LibraProgramming.Game.Towers.Components
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

        protected override void DoFillState(ComponentState state)
        {
        }

        protected override void DoApplyState(ComponentState state)
        {
        }
    }
}