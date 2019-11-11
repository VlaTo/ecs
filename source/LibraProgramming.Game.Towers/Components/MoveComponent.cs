using System.Composition;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Core;
using LibraProgramming.Game.Towers.Extensions;
using System.Globalization;
using System.Numerics;

namespace LibraProgramming.Game.Towers.Components
{
    [Export(typeof(Component))]
    [ExportMetadata("Alias", nameof(MoveComponent))]
    [Component(Alias = nameof(MoveComponent))]
    public sealed class MoveComponent : Component
    {
        /*public Vector2 Position
        {
            get; 
            set;
        }*/

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
            //Position = instance.Position;
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
                //new PropertyState(nameof(Position), Position.ToString()),
                new PropertyState(nameof(Speed), Speed.ToString("F", culture)),
                new PropertyState(nameof(Angle), Angle.ToString("F", culture))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            var culture = CultureInfo.InvariantCulture;

            //Position = state.Properties.GetValue(nameof(Position), VectorConverter.FromString);
            Speed = state.Properties.GetValue<float>(nameof(Speed), formatProvider: culture);
            Angle = state.Properties.GetValue<float>(nameof(Angle), formatProvider: culture);
        }
    }
}