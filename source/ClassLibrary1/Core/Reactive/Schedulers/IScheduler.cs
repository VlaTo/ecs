using System;

namespace ClassLibrary1.Core.Reactive.Schedulers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScheduler
    {
        DateTimeOffset Now
        {
            get;
        }

        IDisposable Schedule(Action action);

        IDisposable Schedule(TimeSpan dueTime, Action action);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISchedulerPeriodic
    {
        IDisposable Schedule(TimeSpan period, Action action);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISchedulerLongRunning
    {
        IDisposable ScheduleLongRunning(Action<ICancelable> action);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISchedulerQueueing
    {
        IDisposable ScheduleQueueing<T>(ICancelable cancel, T state, Action<T> action);
    }
}