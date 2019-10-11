using System;
using System.Threading;

namespace LibraProgramming.Ecs.Core.Reactive.Operators
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    internal abstract class OperatorObserverBase<TSource, TResult> : IDisposable, IObserver<TSource>
    {
        private IDisposable disposable;

        /// <summary>
        /// 
        /// </summary>
        protected IObserver<TResult> Observer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="disposable"></param>
        protected OperatorObserverBase(IObserver<TResult> observer, IDisposable disposable)
        {
            Observer = observer;
            this.disposable = disposable;
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Observer = EmptyObserver<TResult>.Instance;

            var target = Interlocked.Exchange(ref disposable, null);

            if (null != target)
            {
                target.Dispose();
            }
        }

        /// <inheritdoc cref="IObserver{T}.OnCompleted" />
        public abstract void OnCompleted();

        /// <inheritdoc cref="IObserver{T}.OnError" />
        public abstract void OnError(Exception error);

        /// <inheritdoc cref="IObserver{T}.OnNext" />
        public abstract void OnNext(TSource value);
    }
}