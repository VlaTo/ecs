using System;
using ClassLibrary1;

namespace ConsoleApp2.Components
{
    public class UpdateComponent : Component, IDisposable
    {
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
    }
}