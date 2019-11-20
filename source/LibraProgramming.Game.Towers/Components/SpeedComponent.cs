using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Extensions;
using System.Composition;
using System.Globalization;

namespace LibraProgramming.Game.Towers.Components
{
    [Export(typeof(IComponent))]
    [ExportMetadata("Type", nameof(SpeedComponent))]
    [Component(Alias = nameof(SpeedComponent))]
    public sealed class SpeedComponent : Component
    {
        public float Value
        {
            get; 
            set;
        }

        public SpeedComponent()
        {
            Value = 0.0f;
        }

        private SpeedComponent(SpeedComponent instance)
        {
            Value = instance.Value;
        }

        public override IComponent Clone()
        {
            return new SpeedComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            state.Properties = new[]
            {
                new PropertyState(nameof(Value), Value.ToString("F", CultureInfo.InvariantCulture))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Value = state.Properties.GetValue(nameof(Value), value => System.Single.Parse(value, CultureInfo.InvariantCulture));
        }
    }
}