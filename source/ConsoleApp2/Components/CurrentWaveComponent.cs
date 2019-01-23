using System;
using ClassLibrary1;

namespace ConsoleApp2.Components
{
    public class CurrentWaveComponent : Component, IDisposable
    {
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
    }
}