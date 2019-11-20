using System.Composition;
using System.Drawing;
using System.Globalization;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Core;
using LibraProgramming.Game.Towers.Extensions;

namespace LibraProgramming.Game.Towers.Components
{
    [Export(typeof(IComponent))]
    [ExportMetadata("Alias", nameof(MapComponent))]
    [Component(Alias = nameof(MapComponent))]
    public sealed class MapComponent : Component
    {
        public int[] Tiles
        {
            get;
            set;
        }

        public Size Size
        {
            get;
            set;
        }

        public MapComponent()
        {
        }

        private MapComponent(MapComponent instance)
        {
            Tiles = instance.Tiles;
            Size = instance.Size;
        }

        public override IComponent Clone()
        {
            return new MapComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            state.Properties = new[]
            {
                new PropertyState(nameof(Tiles), Tiles.ToString(CultureInfo.InvariantCulture)),
                new PropertyState(nameof(Size), Size.ToString(CultureInfo.InvariantCulture))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Tiles = state.Properties.GetValue(nameof(Tiles), Converters.Tiles);
            Size = state.Properties.GetValue(nameof(Size), Converters.Size);
        }
    }
}