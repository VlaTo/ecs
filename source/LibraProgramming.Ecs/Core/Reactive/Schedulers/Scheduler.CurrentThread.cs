using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace LibraProgramming.Ecs.Core.Reactive.Schedulers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Scheduler
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IScheduler CurrentThread = new CurrentThreadScheduler();

        /// <summary>
        /// 
        /// </summary>
        public static bool IsCurrentThreadSchedulerScheduleRequired => CurrentThreadScheduler.IsScheduleRequired;

        /// <summary>
        /// 
        /// </summary>
        private class CurrentThreadScheduler : IScheduler
        {
            /// <inheritdoc cref="IScheduler.Now" />
            public DateTimeOffset Now => Scheduler.Now;

            [ThreadStatic]
            private static SchedulerQueue queue;

            [ThreadStatic]
            private static Stopwatch stopwatch;

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

            /// <inheritdoc cref="IScheduler.Schedule(System.Action)" />
            public IDisposable Schedule(Action action) => Schedule(TimeSpan.Zero, action);

            /// <inheritdoc cref="IScheduler.Schedule(System.TimeSpan, System.Action)" />
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