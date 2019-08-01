using System;
using ClassLibrary1.Core.Extensions;
using ClassLibrary1.Core.Reactive.Operators;

namespace ClassLibrary1.Core.Reactive
{
    internal class DeferObservable<T> : OperatorObservableBase<T>
    {
        readonly Func<IObservable<T>> func;

        public DeferObservable(Func<IObservable<T>> func)
            : base(false)
        {
            this.func = func;
        }

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

        private class Defer : OperatorObserverBase<T, T>
        {
            public Defer(IObserver<T> observer, IDisposable cancel)
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