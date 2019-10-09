using System;
using ClassLibrary1;

namespace ConsoleApp2.Components
{
    public class UpdateComponent : Component, IDisposable
    {
        public override string Alias => nameof(UpdateComponent);

        public ObservableProperty<TimeSpan> Elapsed
        {
            get;
        }

        public UpdateComponent()
        {
            Elapsed = new ObservableProperty<TimeSpan>(this);
        }

        public void Dispose()
        {
            Elapsed.Release();
        }

        public override ComponentState GetState() =>
            new ComponentState
            {
                Alias = Alias,
                Properties = new PropertyState[0]
            };

        public override IComponent Clone()
        {
            return new UpdateComponent();
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