using System;
using ClassLibrary1.Core.Extensions;
using ClassLibrary1.Core.Reactive.Operators;
using ClassLibrary1.Core.Reactive.Schedulers;

namespace ClassLibrary1.Core.Reactive
{
    internal class EmptyObservable<T> : OperatorObservableBase<T>
    {
        private readonly IScheduler scheduler;

        public EmptyObservable(IScheduler scheduler)
            : base(false)
        {
            this.scheduler = scheduler;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable disposable)
        {
            var empty = new Empty(observer, disposable);

            if (Scheduler.Immediate == scheduler)
            {
                empty.OnCompleted();
                return Disposable.Empty;
            }

            return scheduler.Schedule(empty.OnCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        private class Empty : OperatorObserverBase<T, T>
        {
            public Empty(IObserver<T> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public override void OnNext(T value)
            {
                try
                {
                    Observer.OnNext(value);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public override void OnError(Exception error)
            {
                try
                {
                    Observer.OnError(error);
                }
                finally
                {
                    Dispose();
                }
            }

            public override void OnCompleted()
            {
                try
                {
                    Observer.OnCompleted();
                }
                finally
                {
                    Dispose();
                }
            }
        }
    }
}