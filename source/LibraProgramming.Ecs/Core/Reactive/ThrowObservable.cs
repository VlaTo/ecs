using System;
using LibraProgramming.Ecs.Core.Reactive.Operators;
using LibraProgramming.Ecs.Core.Reactive.Schedulers;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ThrowObservable<T> : OperatorObservableBase<T>
    {
        private readonly Exception exception;
        private readonly IScheduler scheduler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="scheduler"></param>
        public ThrowObservable(Exception exception, IScheduler scheduler)
            : base(Scheduler.CurrentThread == scheduler)
        {
            this.exception = exception;
            this.scheduler = scheduler;
        }

        /// <inheritdoc cref="OperatorObservableBase{T}.SubscribeCore" />
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
            /// <summary>
            /// 
            /// </summary>
            /// <param name="observer"></param>
            /// <param name="cancel"></param>
            public Throw(IObserver<T> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            /// <inheritdoc cref="OperatorObserverBase{TSource,TResult}.OnNext" />
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

            /// <inheritdoc cref="OperatorObserverBase{TSource,TResult}.OnError" />
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

            /// <inheritdoc cref="OperatorObserverBase{TSource,TResult}.OnCompleted" />
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