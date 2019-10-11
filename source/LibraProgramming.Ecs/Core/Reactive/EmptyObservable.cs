using System;
using LibraProgramming.Ecs.Core.Reactive.Operators;
using LibraProgramming.Ecs.Core.Reactive.Schedulers;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class EmptyObservable<T> : OperatorObservableBase<T>
    {
        private readonly IScheduler scheduler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduler"></param>
        public EmptyObservable(IScheduler scheduler)
            : base(false)
        {
            this.scheduler = scheduler;
        }

        /// <inheritdoc cref="OperatorObservableBase{T}.SubscribeCore" />
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
            /// <summary>
            /// 
            /// </summary>
            /// <param name="observer"></param>
            /// <param name="cancel"></param>
            public Empty(IObserver<T> observer, IDisposable cancel)
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