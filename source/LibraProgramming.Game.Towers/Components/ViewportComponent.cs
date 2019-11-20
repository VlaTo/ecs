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

        public string ToString() => ToString("F");

        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "F":
                {
                    return $"{Min:F,formatProvider} {Max:F,formatProvider}";
                }
            }

            return ToString();
        }

        public static Limits Parse(string value) => Parse(value, CultureInfo.InvariantCulture);

        public static Limits Parse(string value, IFormatProvider formatProvider)
        {
            var values = value.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);
            var numbers = values
                .Select(number => System.Single.Parse(number, formatProvider))
                .ToArray();

            if (2 == numbers.Length)
            {
                return new Limits
                {
                    Min = numbers[0],
                    Max = numbers[1]
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
                new PropertyState(nameof(Horizontal), Horizontal.ToString()),
                new PropertyState(nameof(Vertical), Vertical.ToString())
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Horizontal = state.Properties.GetValue(nameof(Horizontal), Limits.Parse);
            Vertical = state.Properties.GetValue(nameof(Vertical), Limits.Parse);
        }
    }
}