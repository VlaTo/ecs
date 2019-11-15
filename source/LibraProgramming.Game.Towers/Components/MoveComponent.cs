using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Extensions;
using System.Composition;
using System.Globalization;

namespace LibraProgramming.Game.Towers.Components
{
    [Component(Alias = nameof(MoveComponent))]
    public sealed class MoveComponent : Component
    {
        public float Speed
        {
            get; 
            set;
        }

        public float Angle
        {
            get;
            set;
        }

        public MoveComponent()
        {
        }

        private MoveComponent(MoveComponent instance)
        {
            Speed = instance.Speed;
            Angle = instance.Angle;
        }

        public override IComponent Clone()
        {
            return new MoveComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            var culture = CultureInfo.InvariantCulture;
            
            state.Properties = new[]
            {
                new PropertyState(nameof(Speed), Speed.ToString("F", culture)),
                new PropertyState(nameof(Angle), Angle.ToString("F", culture))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            var culture = CultureInfo.InvariantCulture;

            Speed = state.Properties.GetValue<float>(nameof(Speed), formatProvider: culture);
            Angle = state.Properties.GetValue<float>(nameof(Angle), formatProvider: culture);
        }
    }
}