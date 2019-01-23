using System;
using ClassLibrary1;

namespace ConsoleApp2.Components
{
    public class GameTimeComponent : Component
    {
        public ObservableProperty<TimeSpan> Elapsed
        {
            get;
        }

        public GameTimeComponent()
        {
            Elapsed = new ObservableProperty<TimeSpan>(this);
        }
    }
}