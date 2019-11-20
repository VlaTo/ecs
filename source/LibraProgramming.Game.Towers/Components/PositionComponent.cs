using System.Globalization;
using System.Numerics;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Core;
using LibraProgramming.Game.Towers.Extensions;

namespace LibraProgramming.Game.Towers.Components
{
    [Component(Alias = nameof(PositionComponent))]
    public sealed class PositionComponent : Component
    {
        public Vector2 Position
        {
            get;
            set;
        }

        public PositionComponent()
        {
        }

        private PositionComponent(PositionComponent instance)
        {
            Position = instance.Position;
        }

        public override IComponent Clone()
        {
            return new PositionComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            state.Properties = new[]
            {
                new PropertyState(nameof(Position), Position.ToString(CultureInfo.InvariantCulture))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Position = state.Properties.GetValue(nameof(Position), Converters.Vector2);
        }
    }
}