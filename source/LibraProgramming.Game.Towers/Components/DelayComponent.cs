using System;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Extensions;

namespace LibraProgramming.Game.Towers.Components
{
    [Component(Alias = nameof(DelayComponent))]
    public class DelayComponent : Component
    {
        public TimeSpan Duration
        {
            get; 
            set;
        }

        public DelayComponent()
        {
        }

        private DelayComponent(DelayComponent instance)
        {
            Duration = instance.Duration;
        }

        public override IComponent Clone()
        {
            return new DelayComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            state.Properties = new[]
            {
                new PropertyState(nameof(Duration), Duration.ToString("c"))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Duration = state.Properties.GetValue(nameof(Duration), TimeSpan.Parse);
        }
    }
}