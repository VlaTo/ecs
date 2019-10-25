using System.Numerics;
using Windows.Services.Store;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Core;
using LibraProgramming.Game.Towers.Extensions;

namespace LibraProgramming.Game.Towers.Components
{
    [Component(Alias = nameof(MoveComponent))]
    public sealed class MoveComponent : Component
    {
        public Vector2 Position
        {
            get; 
            set;
        }

        public Vector2 Speed
        {
            get; 
            set;
        }

        public MoveComponent()
        {
        }

        private MoveComponent(MoveComponent instance)
        {
            Position = instance.Position;
            Speed = instance.Speed;
        }

        public override IComponent Clone()
        {
            return new MoveComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            var str = Position.ToString();
            state.Properties = new[]
            {
                new PropertyState(nameof(Position), str),
                new PropertyState(nameof(Speed), Speed.ToString())
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Position = state.Properties.GetValue(nameof(Position), VectorConverter.FromString);
            Speed = state.Properties.GetValue(nameof(Speed), VectorConverter.FromString);
        }
    }
}