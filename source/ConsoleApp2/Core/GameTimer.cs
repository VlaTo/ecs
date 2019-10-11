using System;
using System.Threading;
using LibraProgramming.Ecs.Core.Reactive;

namespace ConsoleApp2.Core
{
    public class GameTimer : ObservableBase<TimeSpan>, IGameTimer
    {
        private readonly Timer timer;
        private readonly TimeSpan startTimeSpan;

        public GameTimer(TimeSpan delay)
        {
            timer = new Timer(OnTimerTick, null, delay, delay);
            startTimeSpan = TimeSpan.FromMilliseconds(Environment.TickCount);
        }

        private void OnTimerTick(object state)
        {
            var currentTimeSpan = TimeSpan.FromMilliseconds(Environment.TickCount);
            Next(currentTimeSpan - startTimeSpan);
        }
    }
}