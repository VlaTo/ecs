using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Core;
using System;
using System.Collections.Generic;
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
            var points = String.Join(',', WayPoints.Select(point => VectorConverter.ToString(point, culture)));

            state.Properties = new[]
            {
                new PropertyState(nameof(WayPoints), points)
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            var value = Array.Find(state.Properties, info => info.Name.Equals(nameof(WayPoints)));

            if (null == value)
            {
                throw new Exception();
            }

            var culture = CultureInfo.InvariantCulture;
            var values = value.Value.Split(';', StringSplitOptions.RemoveEmptyEntries);
            var list = new List<Vector2>();

            foreach (var str in values)
            {
                list.Add(VectorConverter.FromString(str, culture));
            }

            WayPoints = list.ToArray();
        }
    }
}