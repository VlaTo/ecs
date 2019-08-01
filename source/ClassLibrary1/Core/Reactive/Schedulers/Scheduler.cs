using System;

namespace ClassLibrary1.Core.Reactive.Schedulers
{
    public static partial class Scheduler
    {
        public static class Default
        {
            private static IScheduler constantTime;
            private static IScheduler tailRecursion;
            private static IScheduler iteration;

            public static IScheduler ConstantTimeOperations
            {
                get
                {
                    return constantTime ?? (constantTime = Immediate);
                }
                set
                {
                    constantTime = value;
                }
            }

            public static IScheduler TailRecursion
            {
                get
                {
                    return tailRecursion ?? (tailRecursion = Immediate);
                }
                set
                {
                    tailRecursion = value;
                }
            }

            public static IScheduler Iteration
            {
                get
                {
                    return iteration ?? (iteration = CurrentThread);
                }
                set
                {
                    iteration = value;
                }
            }

            public static void SetDotNetCompatible()
            {
                ConstantTimeOperations = Immediate;
                TailRecursion = Immediate;
                Iteration = CurrentThread;
                //TimeBasedOperations = Scheduler.ThreadPool;
                //AsyncConversions = Scheduler.ThreadPool;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DateTimeOffset Now => DateTimeOffset.Now;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="dueTime"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action action)
        {
            return scheduler.Schedule(dueTime - scheduler.Now, action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
        {
            // InvokeRec1
            var group = new CompositeDisposable(1);
            var gate = new object();

            void RecursiveAction() =>
                action(() =>
                {
                    var isAdded = false;
                    var isDone = false;
                    var d = default(IDisposable);

                    d = scheduler.Schedule(() =>
                    {
                        lock (gate)
                        {
                            if (isAdded)
                            {
                                @group.Remove(d);
                            }
                            else
                            {
                                isDone = true;
                            }
                        }

                        RecursiveAction();
                    });

                    lock (gate)
                    {
                        if (false == isDone)
                        {
                            @group.Add(d);
                            isAdded = true;
                        }
                    }
                });

            group.Add(scheduler.Schedule(RecursiveAction));

            return group;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="dueTime"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action<Action<TimeSpan>> action)
        {
            var group = new CompositeDisposable(1);
            var gate = new object();

            void RecursiveAction() =>
                action(dt =>
                {
                    var isAdded = false;
                    var isDone = false;
                    var d = default(IDisposable);

                    d = scheduler.Schedule(dt, () =>
                    {
                        lock (gate)
                        {
                            if (isAdded)
                            {
                                @group.Remove(d);
                            }
                            else
                            {
                                isDone = true;
                            }
                        }

                        RecursiveAction();
                    });

                    lock (gate)
                    {
                        if (false == isDone)
                        {
                            @group.Add(d);
                            isAdded = true;
                        }
                    }
                });

            group.Add(scheduler.Schedule(dueTime, RecursiveAction));

            return group;
        }

        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action<Action<DateTimeOffset>> action)
        {
            var group = new CompositeDisposable(1);
            var gate = new object();

            void RecursiveAction() =>
                action(dt =>
                {
                    var isAdded = false;
                    var isDone = false;
                    var d = default(IDisposable);

                    d = scheduler.Schedule(dt, () =>
                    {
                        lock (gate)
                        {
                            if (isAdded)
                            {
                                @group.Remove(d);
                            }
                            else
                            {
                                isDone = true;
                            }
                        }

                        RecursiveAction();
                    });

                    lock (gate)
                    {
                        if (false == isDone)
                        {
                            @group.Add(d);
                            isAdded = true;
                        }
                    }
                });

            group.Add(scheduler.Schedule(dueTime, (Action) RecursiveAction));

            return group;
        }
    }
}