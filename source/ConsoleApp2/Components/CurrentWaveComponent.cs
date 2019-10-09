using System;
using ClassLibrary1;

namespace ConsoleApp2.Components
{
    public class CurrentWaveComponent : Component, IDisposable
    {
        public override string Alias { get; }

        public ObservableProperty<TimeSpan> Cooldown
        {
            get;
        }

        public ObservableProperty<int> Number
        {
            get;
        }

        public CurrentWaveComponent()
        {
            Cooldown = new ObservableProperty<TimeSpan>(this);
            Number = new ObservableProperty<int>(this);
        }

        public void Dispose()
        {
            Cooldown.Release();
            Number.Release();
        }

        public override ComponentState GetState()
        {
            throw new NotImplementedException();
        }

        public override IComponent Clone()
        {
            throw new NotImplementedException();
        }

        protected override void DoAttach()
        {
            throw new NotImplementedException();
        }

        protected override void DoRelease()
        {
            throw new NotImplementedException();
        }
    }
}