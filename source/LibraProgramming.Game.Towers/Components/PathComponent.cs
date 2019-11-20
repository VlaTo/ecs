using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Core;
using LibraProgramming.Game.Towers.Extensions;
using System;
using System.Composition;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace LibraProgramming.Game.Towers.Components
{
    [Export(typeof(Component))]
    [ExportMetadata("Alias", nameof(PathComponent))]
    [Component(Alias = nameof(PathComponent))]
    public sealed class PathComponent : Component
    {
        public Vector2[] WayPoints
        {
            get;
            private set;
        }

        public PathComponent()
        {
            WayPoints = Array.Empty<Vector2>();
        }

        private PathComponent(PathComponent instance)
        {
            WayPoints = instance.WayPoints.ToArray();
        }

        public override IComponent Clone()
        {
            return new PathComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            var culture = CultureInfo.InvariantCulture;

            state.Properties = new[]
            {
                new PropertyState(nameof(WayPoints), WayPoints.ToString(culture))
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            WayPoints = state.Properties.GetValue(nameof(WayPoints), Converters.Vectors);
        }
    }
}