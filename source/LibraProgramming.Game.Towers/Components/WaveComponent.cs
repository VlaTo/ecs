using System;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Extensions;

namespace LibraProgramming.Game.Towers.Components
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

        protected override void DoFillState(ComponentState state)
        {
            state.Properties = new[]
            {
                new PropertyState
                {
                    Name = nameof(Number),
                    Value = Convert.ToString(Number)
                }
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Number = state.Properties.GetValue<int>(nameof(Number));
        }
    }
}