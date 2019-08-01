using System;
using System.Threading;

namespace ClassLibrary1.Core.Reactive.Schedulers
{
    public partial class Scheduler
    {
        public static readonly IScheduler Immediate = new ImmediateScheduler();

        private class ImmediateScheduler : IScheduler
        {
            public DateTimeOffset Now => Scheduler.Now;

            public ImmediateScheduler()
            {
            }

            public IDisposable Schedule(Action action)
            {
                action.Invoke();
                return Disposable.Empty;
            }

            public IDisposable Schedule(TimeSpan dueTime, Action action)
            {
                if (dueTime.Ticks > 0)
                {
                    Thread.Sleep(dueTime);
                }

                action.Invoke();

                return Disposable.Empty;
            }
        }
    }
}