using ClassLibrary1;

namespace UnitTestProject1.Components
{
    public class TestComponent : Component
    {
        public override ComponentState GetState()
        {
            return new ComponentState
            {

            };
        }

        protected override void DoAttach()
        {
        }

        protected override void DoRelease()
        {
        }
    }
}
