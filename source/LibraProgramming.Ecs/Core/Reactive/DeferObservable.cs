using System;
using LibraProgramming.Ecs.Core.Reactive.Operators;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DeferObservable<T> : OperatorObservableBase<T>
    {
        readonly Func<IObservable<T>> func;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public DeferObservable(Func<IObservable<T>> func)
            : base(false)
        {
            this.func = func;
        }

        /// <inheritdoc cref="OperatorObservableBase{T}.SubscribeCore" />
        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            var defer = new Defer(observer, cancel);
            IObservable<T> source;

            try
            {
                source = func.Invoke();
            }
            catch (Exception exception)
            {
                source = Observable.Throw<T>(exception);
            }

            return source.Subscribe(defer);
        }

        /// <summary>
        /// 
        /// </summary>
        private class Defer : OperatorObserverBase<T, T>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="observer"></param>
            /// <param name="cancel"></param>
            public Defer(IObserver<T> observer, IDisposable cancel)
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