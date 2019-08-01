using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace ClassLibrary1.Core.Reactive.Schedulers
{
    public partial class Scheduler
    {
        public static readonly IScheduler CurrentThread = new CurrentThreadScheduler();

        public static bool IsCurrentThreadSchedulerScheduleRequired => CurrentThreadScheduler.IsScheduleRequired;

        private class CurrentThreadScheduler : IScheduler
        {
            public DateTimeOffset Now => Scheduler.Now;

            [ThreadStatic]
            static SchedulerQueue queue;

            [ThreadStatic]
            static Stopwatch stopwatch;

            private static SchedulerQueue GetQueue()
            {
                return queue;
            }

            private static void SetQueue(SchedulerQueue newQueue)
            {
                queue = newQueue;
            }

            private static TimeSpan Time
            {
                get
                {
                    if (null == stopwatch)
                    {
                        stopwatch = Stopwatch.StartNew();
                    }

                    return stopwatch.Elapsed;
                }
            }

            /// <summary>
            /// Gets a value that indicates whether the caller must call a Schedule method.
            /// </summary>
            [EditorBrowsable(EditorBrowsableState.Advanced)]
            public static bool IsScheduleRequired => null == GetQueue();

            public IDisposable Schedule(Action action) => Schedule(TimeSpan.Zero, action);

            public IDisposable Schedule(TimeSpan dueTime, Action action)
            {
                if (null == action)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                //var dt = Time + Scheduler.Normalize(dueTime);
                var scheduledItem = new ScheduledItem(action, Time + dueTime);
                var queue = GetQueue();

                if (null == queue)
                {
                    queue = new SchedulerQueue(4);

                    queue.Enqueue(scheduledItem);

                    SetQueue(queue);

                    try
                    {
                        Trampoline.Run(queue);
                    }
                    finally
                    {
                        SetQueue(null);
                    }
                }
                else
                {
                    queue.Enqueue(scheduledItem);
                }

                return scheduledItem.Cancellation;
            }

            /// <summary>
            /// 
            /// </summary>
            private static class Trampoline
            {
                public static void Run(SchedulerQueue queue)
                {
                    while (0 < queue.Count)
                    {
                        var item = queue.Dequeue();

                        if (item.IsCanceled)
                        {
                            continue;
                        }

                        var wait = item.DueTime - Time;

                        if (wait.Ticks > 0)
                        {
                            Thread.Sleep(wait);
                        }

                        if (false == item.IsCanceled)
                        {
                            item.Invoke();
                        }
                    }
                }
            }
        }
    }
}