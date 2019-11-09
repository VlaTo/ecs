using System;

namespace LibraProgramming.Ecs.Core.Reactive.Schedulers
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Scheduler
    {
        /// <summary>
        /// 
        /// </summary>
        public static class Default
        {
            private static IScheduler constantTime;
            private static IScheduler tailRecursion;
            private static IScheduler iteration;

            /// <summary>
            /// 
            /// </summary>
            public static IScheduler ConstantTimeOperations
            {
                get => constantTime ?? (constantTime = Scheduler.Immediate);
                set => constantTime = value;
            }

            /// <summary>
            /// 
            /// </summary>
            public static IScheduler TailRecursion
            {
                get => tailRecursion ?? (tailRecursion = Scheduler.Immediate);
                set => tailRecursion = value;
            }

            /// <summary>
            /// 
            /// </summary>
            public static IScheduler Iteration
            {
                get => iteration ?? (iteration = CurrentThread);
                set => iteration = value;
            }

            /// <summary>
            /// 
            /// </summary>
            public static void SetDotNetCompatible()
            {
                ConstantTimeOperations = Scheduler.Immediate;
                TailRecursion = Scheduler.Immediate;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="dueTime"></param>
        /// <param name="action"></param>
        /// <returns></returns>
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