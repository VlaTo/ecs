using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Extensions;

namespace LibraProgramming.Game.Towers.Components
{
    public struct Limits
    {
        public float Min
        {
            get; 
            set;
        }

        public float Max
        {
            get; 
            set;
        }

        public string ToString(string format)
        {
            switch (format)
            {
                case "F":
                {
                    var culture = CultureInfo.InvariantCulture;
                    return $"{Min:F,culture} {Max:F,culture}";
                }
            }

            return ToString();
        }

        public static Limits Parse(string value)
        {
            var values = value.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);
            var culture = CultureInfo.InvariantCulture;
            var temp = values
                .Select(val => System.Single.Parse(val, culture))
                .ToArray();

            if (2 == temp.Length)
            {
                return new Limits
                {
                    Min = temp[0],
                    Max = temp[1]
                };
            }

            throw new InvalidOperationException();
        }
    }

    [Component(Alias = nameof(ViewportComponent))]
    public sealed class ViewportComponent : Component
    {
        public Limits Horizontal
        {
            get; 
            set;
        }
        public Limits Vertical
        {
            get; 
            set;
        }

        public ViewportComponent()
        {
        }

        private ViewportComponent(ViewportComponent instance)
        {
            /*Horizontal = new Limits
            {
                Min = instance.Horizontal.Min, 
                Max = instance.Horizontal.Max
            };*/

            Horizontal = instance.Horizontal;
            Vertical = instance.Vertical;
        }
        
        public override IComponent Clone()
        {
            return new ViewportComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            state.Properties = new[]
            {
                new PropertyState(nameof(Horizontal), Horizontal.ToString("F")),
                new PropertyState(nameof(Vertical), Vertical.ToString("F"))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Horizontal = state.Properties.GetValue<Limits>(nameof(Horizontal), Limits.Parse);
            Vertical = state.Properties.GetValue<Limits>(nameof(Vertical), Limits.Parse);
        }
    }
}