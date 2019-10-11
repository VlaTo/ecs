using System.Numerics;
using LibraProgramming.Ecs;

namespace ConsoleApp2.Components
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

        protected override void DoAttach()
        {
            ;
        }

        protected override void DoRelease()
        {
            ;
        }
    }
}