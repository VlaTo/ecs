using ClassLibrary1;

namespace UnitTestProject1.Components
{
    public class TestComponent : Component
    {
        public override string Alias => nameof(TestComponent);

        public TestComponent()
        {
        }

        private TestComponent(TestComponent other)
        {
        }

        public override ComponentState GetState()
        {
            return new ComponentState
            {
                Alias = Alias,
                Properties = new PropertyState[0]
            };
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
