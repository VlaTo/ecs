using System;
using ClassLibrary1.Core.Reactive;
using ClassLibrary1.Core.Reactive.Collections;

namespace ClassLibrary1.Extensions
{
    internal static class ObservableCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="onAdded"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(
            this IObservableCollection<T> source,
            Action<T> onAdded) => source.Subscribe(
            CollectionObserver.CreateSubscribeObserver(
                onAdded,
                Stubs<T>.Ignore,
                Stubs.Throw,
                Stubs.Nop)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(
            this IObservableCollection<T> source,
            Action<T> onAdded,
            Action<T> onRemoved) => source.Subscribe(
            CollectionObserver.CreateSubscribeObserver(
                onAdded,
                onRemoved,
                Stubs.Throw,
                Stubs.Nop)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(
            this IObservableCollection<T> source,
            Action<T> onAdded,
            Action<T> onRemoved,
            Action onCompleted) => source.Subscribe(
            CollectionObserver.CreateSubscribeObserver(
                onAdded,
                onRemoved,
                Stubs.Throw,
                onCompleted)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="source"></param>
        /// <param name="state"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T, TState>(
            this IObservableCollection<T> source,
            TState state,
            Action<T, TState> onAdded,
            Action<T, TState> onRemoved) => source.Subscribe(
            CollectionObserver.Create(
                state,
                onAdded,
                onRemoved,
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
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T, TState>(
            this IObservableCollection<T> source,
            TState state,
            Action<T, TState> onAdded,
            Action<T, TState> onRemoved,
            Action<TState> onCompleted) => source.Subscribe(
            CollectionObserver.Create(
                state,
                onAdded,
                onRemoved,
                Stubs<TState>.Throw,
                onCompleted)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(this IObservableCollection<T> source) =>
           source.Subscribe(CollectionThrowObserver<T>.Instance);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(
            this IObservableCollection<T> source,
            Action<T> onAdded,
            Action<T> onRemoved,
            Action<Exception> onError) => source.Subscribe(
            CollectionObserver.CreateSubscribeObserver(
                onAdded,
                onRemoved,
                onError,
                Stubs.Nop)
        );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onError"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T>(
            this IObservableCollection<T> source,
            Action<T> onAdded,
            Action<T> onRemoved,
            Action<Exception> onError,
            Action onCompleted) => source.Subscribe(
            CollectionObserver.CreateSubscribeObserver(
                onAdded,
                onRemoved,
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
        /// <param name="onAdded"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<T, TState>(
            this IObservableCollection<T> source,
            TState state,
            Action<T, TState> onAdded)
            => source.Subscribe(
                CollectionObserver.CreateSubscribeObserver(
                    state,
                    onAdded,
                    Stubs<T, TState>.Ignore,
                    Stubs<TState>.Throw,
                    Stubs<TState>.Ignore)
            );
    }
}