using System;
using LibraProgramming.Ecs.Core.Reactive.Schedulers;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    public static class Observable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscribe"></param>
        /// <returns></returns>
        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe)
        {
            if (null == subscribe)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return new CreateObservable<T>(subscribe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscribe"></param>
        /// <param name="isRequiredSubscribeOnCurrentThread"></param>
        /// <returns></returns>
        public static IObservable<T> Create<T>(Func<IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
        {
            if (null == subscribe)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return new CreateObservable<T>(subscribe, isRequiredSubscribeOnCurrentThread);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <param name="subscribe"></param>
        /// <returns></returns>
        public static IObservable<T> CreateWithState<T, TState>(TState state, Func<TState, IObserver<T>, IDisposable> subscribe)
        {
            if (null == subscribe)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return new CreateObservable<T, TState>(state, subscribe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <param name="subscribe"></param>
        /// <param name="isRequiredSubscribeOnCurrentThread"></param>
        /// <returns></returns>
        public static IObservable<T> CreateWithState<T, TState>(TState state, Func<TState, IObserver<T>, IDisposable> subscribe, bool isRequiredSubscribeOnCurrentThread)
        {
            if (null == subscribe)
            {
                throw new ArgumentNullException(nameof(subscribe));
            }

            return new CreateObservable<T, TState>(state, subscribe, isRequiredSubscribeOnCurrentThread);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObservable<T> Empty<T>()
        {
            return Empty<T>(Scheduler.Default.ConstantTimeOperations);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static IObservable<T> Empty<T>(IScheduler scheduler)
        {
            if (scheduler == Scheduler.Immediate)
            {
                return ImmutableEmptyObservable<T>.Instance;
            }

            return new EmptyObservable<T>(scheduler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="witness"></param>
        /// <returns></returns>
        public static IObservable<T> Empty<T>(T witness)
        {
            return Empty<T>(Scheduler.Default.ConstantTimeOperations);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scheduler"></param>
        /// <param name="witness"></param>
        /// <returns></returns>
        public static IObservable<T> Empty<T>(IScheduler scheduler, T witness)
        {
            return Empty<T>(scheduler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IObservable<T> Throw<T>(Exception error)
        {
            return Throw<T>(error, Scheduler.Default.ConstantTimeOperations);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error"></param>
        /// <param name="witness"></param>
        /// <returns></returns>
        public static IObservable<T> Throw<T>(Exception error, T witness)
        {
            return Throw<T>(error, Scheduler.Default.ConstantTimeOperations);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler)
        {
            return new ThrowObservable<T>(error, scheduler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error"></param>
        /// <param name="scheduler"></param>
        /// <param name="witness"></param>
        /// <returns></returns>
        public static IObservable<T> Throw<T>(Exception error, IScheduler scheduler, T witness)
        {
            return Throw<T>(error, scheduler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="disposable"></param>
        /// <returns></returns>
        internal static IObservable<T> AddRef<T>(IObservable<T> source, RefCountDisposable disposable)
        {
            return Create<T>(
                observer => new CompositeDisposable(disposable.GetDisposable(), source.Subscribe(observer))
            );
        }
    }
}