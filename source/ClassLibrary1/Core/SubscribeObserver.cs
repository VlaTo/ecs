using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClassLibrary1.Core
{
    internal sealed class SubscribeObserver<TValue> : IObserver<TValue>, IDisposable
    {
        private readonly Action<TValue> onNext;
        private readonly Action onCompleted;
        private readonly Action<Exception> onError;
        private readonly ICollection<IDisposable> subscriptions;
        private bool disposed;

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

        void IObserver<TValue>.OnCompleted()
        {
            onCompleted?.Invoke();
            Dispose(true);
        }

        void IObserver<TValue>.OnError(Exception error)
        {
            onError?.Invoke(error);
            Dispose(true);
        }

        void IObserver<TValue>.OnNext(TValue value)
        {
            onNext.Invoke(value);
        }

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