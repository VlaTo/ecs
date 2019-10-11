using LibraProgramming.Ecs;

namespace ConsoleApp2.Components
{
    [Component(Alias = nameof(WaveComponent))]
    public sealed class WaveComponent : Component
    {
        public int Number
        {
            get;
            set;
        }

        public WaveComponent()
        {
        }

        private WaveComponent(WaveComponent instance)
        {
            Number = instance.Number;
        }

        public override IComponent Clone()
        {
            return new WaveComponent(this);
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