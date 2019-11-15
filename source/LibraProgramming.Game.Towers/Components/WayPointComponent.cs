using LibraProgramming.Ecs;
using System.Composition;
using System.Globalization;

namespace LibraProgramming.Game.Towers.Components
{
    [Export(typeof(Component))]
    [ExportMetadata("Alias", nameof(WayPointComponent))]
    [Component(Alias = nameof(WayPointComponent))]
    public sealed class WayPointComponent : Component
    {
        public WayPointComponent()
        {
        }

        private WayPointComponent(WayPointComponent instance)
        {
        }

        public override IComponent Clone()
        {
            return new WayPointComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            var culture = CultureInfo.InvariantCulture;

            state.Properties = new PropertyState [0];
            /*{
                new PropertyState(nameof(Position), Position.ToString("G", culture))
            };*/
        }

        protected override void DoApplyState(ComponentState state)
        {
            var culture = CultureInfo.InvariantCulture;
            //Position = state.Properties.GetValue(nameof(Position), VectorConverter.FromString);
        }
    }
}