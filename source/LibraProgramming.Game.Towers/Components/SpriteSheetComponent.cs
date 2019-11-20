using System;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Core;
using LibraProgramming.Game.Towers.Extensions;
using System.Composition;
using System.Globalization;
using System.Linq;
using Windows.Graphics.Imaging;

namespace LibraProgramming.Game.Towers.Components
{
    public struct TileSize
    {
        public uint Width
        {
            get;
            set;
        }

        public uint Height
        {
            get;
            set;
        }

        public string ToString() => ToString("D");

        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "D":
                {
                    return $"{Width:D,formatProvider} {Height:D,formatProvider}";
                }
            }

            return ToString();
        }

        public static TileSize Parse(string value) => Parse(value, CultureInfo.InvariantCulture);

        public static TileSize Parse(string value, IFormatProvider formatProvider)
        {
            var values = value.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
            var numbers = values
                .Select(number => System.UInt32.Parse(number, formatProvider))
                .ToArray();

            if (2 == numbers.Length)
            {
                return new TileSize
                {
                    Width = numbers[0],
                    Height = numbers[1]
                };
            }

            throw new InvalidOperationException();

        }
    }

    [Export(typeof(IComponent))]
    [ExportMetadata("Alias", nameof(SpriteSheetComponent))]
    [Component(Alias = nameof(SpriteSheetComponent))]
    public sealed class SpriteSheetComponent : Component
    {
        public string Source
        {
            get; 
            set;
        }

        public TileSize TileSize
        {
            get;
            set;
        }

        public int TilesCount
        {
            get;
            set;
        }

        [ImportingConstructor]
        public SpriteSheetComponent()
        {
        }

        private SpriteSheetComponent(SpriteSheetComponent instance)
        {
            Source = instance.Source;
            TileSize = instance.TileSize;
            TilesCount = instance.TilesCount;
        }

        public override IComponent Clone()
        {
            return new SpriteSheetComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            state.Properties = new[]
            {
                new PropertyState(nameof(Source), Source),
                new PropertyState(nameof(TileSize),  TileSize.ToString()),
                new PropertyState(nameof(TilesCount), TilesCount.ToString("D", CultureInfo.InvariantCulture))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Source = state.Properties.GetValue<string>(nameof(Source));
            TileSize = state.Properties.GetValue(nameof(TileSize), TileSize.Parse);
            TilesCount = state.Properties.GetValue<int>(nameof(TilesCount), value => System.Int32.Parse(value, CultureInfo.InvariantCulture));
        }
    }
}