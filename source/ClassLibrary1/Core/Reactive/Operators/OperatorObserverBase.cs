using System;
using System.Threading;

namespace ClassLibrary1.Core.Reactive.Operators
{
    internal abstract class OperatorObserverBase<TSource, TResult> : IDisposable, IObserver<TSource>
    {
        private IDisposable disposable;

        protected IObserver<TResult> Observer;

        protected OperatorObserverBase(IObserver<TResult> observer, IDisposable disposable)
        {
            Observer = observer;
            this.disposable = disposable;
        }

        public void Dispose()
        {
            Observer = EmptyObserver<TResult>.Instance;

            var target = Interlocked.Exchange(ref disposable, null);

            if (null != target)
            {
                target.Dispose();
            }
        }

        public abstract void OnCompleted();

        public abstract void OnError(Exception error);

        public abstract void OnNext(TSource value);
    }
}