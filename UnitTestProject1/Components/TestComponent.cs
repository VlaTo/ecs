using System.Collections.ObjectModel;
using ClassLibrary1;

namespace UnitTestProject1.Components
{
    public class TestComponent : Component
    {
        public override string Alias => nameof(TestComponent);

        public ObservableProperty<int> TestProperty
        {
            get;
        }

        public TestComponent()
        {
            TestProperty = new ObservableProperty<int>(this);
        }

        private TestComponent(TestComponent instance)
        {
        }

        public override ComponentState GetState()
        {
            return new ComponentState
            {
                Alias = Alias,
                Properties = new[]
                {
                    new PropertyState
                    {
                        Name = nameof(TestProperty),
                        Value = TestProperty.Value.ToString()
                    }
                }
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
