using ClassLibrary1;

namespace UnitTestProject1.Components
{
    public class TestComponent : Component
    {
        public override string Alias => nameof(TestComponent);

        public TestComponent()
        {
        }

        private TestComponent(TestComponent instance)
        {
        }

        public override ComponentState GetState()
        {
            return new ComponentState
            {

            };
        }

        public override void SetState(ComponentState state)
        {
            throw new System.NotImplementedException();
        }

        public override IComponent Clone()
        {
            return new TestComponent(this);
        }

        protected override void DoAttach()
        {
        }

        protected override void DoRelease()
        {
        }
    }
}
