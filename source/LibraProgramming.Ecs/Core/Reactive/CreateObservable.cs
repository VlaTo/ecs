using System;
using LibraProgramming.Ecs.Core.Reactive.Operators;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CreateObservable<T> : OperatorObservableBase<T>
    {
        private readonly Func<IObserver<T>, IDisposable> subscribe;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscribe"></param>
        public CreateObservable(Func<IObserver<T>, IDisposable> subscribe)
            : this(subscribe, true)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscribe"></param>
        /// <param name="isRequiredSubscribeOnCurrentThread"></param>
        public CreateObservable(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
            : base(isRequiredSubscribeOnCurrentThread)
        {
            this.subscribe = subscribe;
        }

        /// <inheritdoc cref="OperatorObservableBase{T}.SubscribeCore" />
        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable disposable)
        {
            observer = new Create(observer, disposable);
            return subscribe(observer) ?? Disposable.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Create : OperatorObserverBase<T, T>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="observer"></param>
            /// <param name="cancel"></param>
            public Create(IObserver<T> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            /// <inheritdoc cref="OperatorObserverBase{TSource,TResult}.OnNext" />
            public override void OnNext(T value)
            {
                Observer.OnNext(value);
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TState"></typeparam>
    internal class CreateObservable<T, TState> : OperatorObservableBase<T>
    {
        private readonly TState state;
        private readonly Func<TState, IObserver<T>, IDisposable> subscribe;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="subscribe"></param>
        public CreateObservable(TState state, Func<TState, IObserver<T>, IDisposable> subscribe)
            : base(true) // fail safe
        {
            this.state = state;
            this.subscribe = subscribe;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="subscribe"></param>
        /// <param name="isRequiredSubscribeOnCurrentThread"></param>
        public CreateObservable(TState state, Func<TState, IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
            : base(isRequiredSubscribeOnCurrentThread)
        {
            this.state = state;
            this.subscribe = subscribe;
        }

        /// <inheritdoc cref="OperatorObservableBase{T}.SubscribeCore" />
        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            observer = new Create(observer, cancel);
            return subscribe(state, observer) ?? Disposable.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Create : OperatorObserverBase<T, T>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="observer"></param>
            /// <param name="cancel"></param>
            public Create(IObserver<T> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            /// <inheritdoc cref="OperatorObserverBase{TSource,TResult}.OnNext" />
            public override void OnNext(T value)
            {
                Observer.OnNext(value);
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