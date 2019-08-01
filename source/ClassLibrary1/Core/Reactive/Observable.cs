using System;
using ClassLibrary1.Core.Reactive.Schedulers;

namespace ClassLibrary1.Core.Reactive
{
    public static class Observable
    {
        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe)
        {
            if (null == subscribe)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return new CreateObservable<T>(subscribe);
        }

        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
        {
            if (null == subscribe)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return new CreateObservable<T>(subscribe, isRequiredSubscribeOnCurrentThread);
        }

        public static IObservable<T> CreateWithState<T, TState>(TState state, Func<TState, IObserver<T>, IDisposable> subscribe)
        {
            if (null == subscribe)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return new CreateObservable<T, TState>(state, subscribe);
        }

        public static IObservable<T> CreateWithState<T, TState>(TState state, Func<TState, IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
        {
            if (null == subscribe)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return new CreateObservable<T, TState>(state, subscribe, isRequiredSubscribeOnCurrentThread);
        }

        public static IObservable<T> Empty<T>()
        {
            return Empty<T>(Scheduler.Default.ConstantTimeOperations);
        }

        public static IObservable<T> Empty<T>(IScheduler scheduler)
        {
            if (scheduler == Scheduler.Immediate)
            {
                return ImmutableEmptyObservable<T>.Instance;
            }

            return new EmptyObservable<T>(scheduler);
        }

        public static IObservable<T> Empty<T>(T witness)
        {
            return Empty<T>(Scheduler.Default.ConstantTimeOperations);
        }

        public static IObservable<T> Empty<T>(IScheduler scheduler, T witness)
        {
            return Empty<T>(scheduler);
        }

        public static IObservable<T> Throw<T>(Exception error)
        {
            return Throw<T>(error, Scheduler.Default.ConstantTimeOperations);
        }

        public static IObservable<T> Throw<T>(Exception error, T witness)
        {
            return Throw<T>(error, Scheduler.Default.ConstantTimeOperations);
        }

        public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler)
        {
            return new ThrowObservable<T>(error, scheduler);
        }

        public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler, T witness)
        {
            return Throw<T>(error, scheduler);
        }

        internal static IObservable<T> AddRef<T>(IObservable<T> source, RefCountDisposable disposable)
        {
            return Create<T>(
                observer => new CompositeDisposable(disposable.GetDisposable(), source.Subscribe(observer))
            );
        }
    }
}