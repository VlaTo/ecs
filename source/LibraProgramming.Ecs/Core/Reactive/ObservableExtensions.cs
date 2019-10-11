using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(this IObservable<T> source) =>
            source.Subscribe(ThrowObserver<T>.Instance);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="onNext"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(
            this IObservable<T> source,
            Action<T> onNext) => source.Subscribe(
            Observer.CreateSubscribeObserver(
                onNext,
                Stubs.Throw,
                Stubs.Nop)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="onNext"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Action<Exception> onError) => source.Subscribe(
            Observer.CreateSubscribeObserver(
                onNext,
                onError,
                Stubs.Nop)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="onNext"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Action onCompleted) => source.Subscribe(
            Observer.CreateSubscribeObserver(
                onNext,
                Stubs.Throw,
                onCompleted)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="onNext"></param>
        /// <param name="onError"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(
            this IObservable<T> source,
            Action<T> onNext,
            Action<Exception> onError,
            Action onCompleted) => source.Subscribe(
            Observer.CreateSubscribeObserver(
                onNext,
                onError,
                onCompleted)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="source"></param>
        /// <param name="state"></param>
        /// <param name="onNext"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T, TState>(
            this IObservable<T> source,
            TState state,
            Action<T, TState> onNext) => source.Subscribe(
            Observer.CreateSubscribeObserver(
                state,
                onNext,
                Stubs<TState>.Throw,
                Stubs<TState>.Ignore)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="source"></param>
        /// <param name="state"></param>
        /// <param name="onNext"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T, TState>(
            this IObservable<T> source,
            TState state,
            Action<T, TState> onNext,
            Action<TState> onCompleted) => source.Subscribe(
            Observer.CreateSubscribeObserver(
                state,
                onNext,
                Stubs<TState>.Throw,
                Stubs<TState>.Ignore)
        );
    }
}