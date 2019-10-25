using System.Numerics;
using LibraProgramming.Ecs;
using LibraProgramming.Game.Towers.Core;
using LibraProgramming.Game.Towers.Extensions;

namespace LibraProgramming.Game.Towers.Components
{
    [Component(Alias = nameof(TransformComponent))]
    public sealed class TransformComponent : Component
    {
        public Matrix3x2 Transform
        {
            get; 
            set;
        }

        public TransformComponent()
        {
            Transform = Matrix3x2.Identity;
        }

        private TransformComponent(TransformComponent instance)
        {
            Transform = instance.Transform;
        }

        public override IComponent Clone()
        {
            return new TransformComponent(this);
        }

        protected override void DoFillState(ComponentState state)
        {
            var str = Transform.ToString();
            state.Properties = new[]
            {
                new PropertyState(nameof(Transform), str)
            };
        }

        protected override void DoApplyState(ComponentState state)
        {
            Transform = state.Properties.GetValue(nameof(Transform), MatrixConverter.FromString);
        }
    }
}