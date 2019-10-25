using System.Globalization;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Extensions;

namespace LibraProgramming.Game.Towers.Components
{
    [Component(Alias = nameof(ViewportComponent))]
    public sealed class ViewportComponent : Component
    {
        public float Width
        {
            get;
            set;
        }

        public float Height
        {
            get; 
            set;
        }

        public ViewportComponent()
        {
        }

        private ViewportComponent(ViewportComponent instance)
        {
            Width = instance.Width;
            Height = instance.Height;
        }
        
        public override IComponent Clone()
        {
            return new ViewportComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            var culture = CultureInfo.InvariantCulture;
            state.Properties = new[]
            {
                new PropertyState(nameof(Width), Width.ToString(culture)),
                new PropertyState(nameof(Height), Height.ToString(culture))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Width = state.Properties.GetValue<float>(nameof(Width));
            Height = state.Properties.GetValue<float>(nameof(Height));
        }
    }
}