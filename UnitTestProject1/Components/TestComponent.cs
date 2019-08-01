using ClassLibrary1;

namespace UnitTestProject1.Components
{
    [Component(Alias = "Test")]
    public class TestComponent : Component, IComponentStateApply
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

        public void Apply(ComponentState state)
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
