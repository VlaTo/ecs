using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    internal sealed class SubscribeObserver<TValue> : IObserver<TValue>, IDisposable
    {
        private readonly Action<TValue> onNext;
        private readonly Action onCompleted;
        private readonly Action<Exception> onError;
        private readonly ICollection<IDisposable> subscriptions;
        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onNext"></param>
        /// <param name="onCompleted"></param>
        /// <param name="onError"></param>
        public SubscribeObserver(Action<TValue> onNext, Action onCompleted = null, Action<Exception> onError = null)
        {
            if (null == onNext)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.onError = onError;

            subscriptions = new Collection<IDisposable>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observable"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObservable<TValue> observable)
        {
            if (null == observable)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            var subscription = observable.Subscribe(this);

            subscriptions.Add(subscription);

            return subscription;
        }

        /// <inheritdoc cref="IObserver{T}.OnCompleted" />
        void IObserver<TValue>.OnCompleted()
        {
            onCompleted?.Invoke();
            Dispose(true);
        }

        /// <inheritdoc cref="IObserver{T}.OnError" />
        void IObserver<TValue>.OnError(Exception error)
        {
            onError?.Invoke(error);
            Dispose(true);
        }

        /// <inheritdoc cref="IObserver{T}.OnNext" />
        void IObserver<TValue>.OnNext(TValue value)
        {
            onNext.Invoke(value);
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        void IDisposable.Dispose()
        {
            onCompleted?.Invoke();
            Dispose(true);
        }

        private void ReleaseSubscriptions()
        {
            var disposables = subscriptions.ToArray();

            subscriptions.Clear();

            for (var index = disposables.Length - 1; 0 <= index; index--)
            {
                var subscription = disposables[index];
                subscription.Dispose();
            }
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    ReleaseSubscriptions();
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}