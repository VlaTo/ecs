using LibraProgramming.Ecs;

namespace ConsoleApp2.Components
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
        }
        
        public override IComponent Clone()
        {
            throw new System.NotImplementedException();
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