using System;
using System.Threading;

namespace LibraProgramming.Ecs.Core.Reactive.Schedulers
{
    public partial class Scheduler
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IScheduler Immediate = new ImmediateScheduler();

        /// <summary>
        /// 
        /// </summary>
        private class ImmediateScheduler : IScheduler
        {
            /// <inheritdoc cref="IScheduler.Now" />
            public DateTimeOffset Now => Scheduler.Now;

            /// <summary>
            /// 
            /// </summary>
            public ImmediateScheduler()
            {
            }

            /// <inheritdoc cref="IScheduler.Schedule(System.Action)" />
            public IDisposable Schedule(Action action)
            {
                action.Invoke();
                return Disposable.Empty;
            }

            /// <inheritdoc cref="IScheduler.Schedule(System.TimeSpan, System.Action)" />
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