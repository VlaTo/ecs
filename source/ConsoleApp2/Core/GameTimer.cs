using System;
using System.Threading;
using ClassLibrary1.Core.Reactive;

namespace ConsoleApp2.Core
{
    public class GameTimer : ObservableBase<TimeSpan>, IGameTimer
    {
        private readonly Timer timer;

        public GameTimer(TimeSpan delay)
        {
            timer = new Timer(OnTimerTick, null, delay, delay);
        }

        private void OnTimerTick(object state)
        {
            var current = TimeSpan.FromMilliseconds(Environment.TickCount);
            Next(current);
        }
    }
}