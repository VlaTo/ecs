using System;
using ClassLibrary1.Core.Extensions;
using ClassLibrary1.Core.Reactive.Operators;

namespace ClassLibrary1.Core.Reactive
{
    internal sealed class CreateObservable<T> : OperatorObservableBase<T>
    {
        private readonly Func<IObserver<T>, IDisposable> subscribe;

        public CreateObservable(Func<IObserver<T>, IDisposable> subscribe)
            : this(subscribe, true)
        {
        }

        public CreateObservable(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
            : base(isRequiredSubscribeOnCurrentThread)
        {
            this.subscribe = subscribe;
        }

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
            public Create(IObserver<T> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public override void OnNext(T value)
            {
                Observer.OnNext(value);
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

    internal class CreateObservable<T, TState> : OperatorObservableBase<T>
    {
        private readonly TState state;
        private readonly Func<TState, IObserver<T>, IDisposable> subscribe;

        public CreateObservable(TState state, Func<TState, IObserver<T>, IDisposable> subscribe)
            : base(true) // fail safe
        {
            this.state = state;
            this.subscribe = subscribe;
        }

        public CreateObservable(TState state, Func<TState, IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
            : base(isRequiredSubscribeOnCurrentThread)
        {
            this.state = state;
            this.subscribe = subscribe;
        }

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
            public Create(IObserver<T> observer, IDisposable cancel)
                : base(observer, cancel)
            {
            }

            public override void OnNext(T value)
            {
                Observer.OnNext(value);
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