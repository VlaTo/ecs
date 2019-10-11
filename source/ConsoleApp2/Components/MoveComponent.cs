using LibraProgramming.Ecs;
using System.Numerics;

namespace ConsoleApp2.Components
{
    [Component(Alias = nameof(MoveComponent))]
    public sealed class MoveComponent : Component
    {
        public Vector2 Position
        {
            get; 
            set;
        }

        public Vector2 Speed
        {
            get; 
            set;
        }

        public MoveComponent()
        {
        }

        private MoveComponent(MoveComponent instance)
        {
            Position = instance.Position;
            Speed = instance.Speed;
        }

        public override IComponent Clone()
        {
            return new MoveComponent(this);
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