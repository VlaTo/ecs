using System;
using ClassLibrary1.Core.Extensions;
using ClassLibrary1.Core.Reactive.Operators;
using ClassLibrary1.Core.Reactive.Schedulers;

namespace ClassLibrary1.Core.Reactive
{
    internal sealed class ThrowObservable<T> : OperatorObservableBase<T>
    {
        private readonly Exception exception;
        private readonly IScheduler scheduler;

        public ThrowObservable(Exception exception, IScheduler scheduler)
            : base(Scheduler.CurrentThread == scheduler)
        {
            this.exception = exception;
            this.scheduler = scheduler;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable disposable)
        {
            var o = new Throw(observer, disposable);

            if (Scheduler.Immediate == scheduler)
            {
                observer.OnError(exception);
                return Disposable.Empty;
            }

            return scheduler.Schedule(() =>
            {
                observer.OnError(exception);
                observer.OnCompleted();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private class Throw : OperatorObserverBase<T, T>
        {
            public Throw(IObserver<T> observer, IDisposable cancel)
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