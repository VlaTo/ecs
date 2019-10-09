using System;

namespace ClassLibrary1.Core.Reactive
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> source) =>
            source.Subscribe(ThrowObserver<T>.Instance);

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext) =>
            source.Subscribe(Observer.CreateSubscribeObserver(onNext, Stubs.Throw, Stubs.Nop));

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError) =>
            source.Subscribe(Observer.CreateSubscribeObserver(onNext, onError, Stubs.Nop));

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted) =>
            source.Subscribe(Observer.CreateSubscribeObserver(onNext, Stubs.Throw, onCompleted));

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted) =>
            source.Subscribe(Observer.CreateSubscribeObserver(onNext, onError, onCompleted));
        
        public static IDisposable Subscribe<T, TState>(this IObservable<T> source, TState state, Action<T, TState> onNext) =>
            source.Subscribe(Observer.CreateSubscribeObserver(state, onNext, Stubs<TState>.Throw, Stubs<TState>.Ignore));
        
        public static IDisposable Subscribe<T, TState>(this IObservable<T> source, TState state, Action<T, TState> onNext, Action<TState> onCompleted) =>
            source.Subscribe(Observer.CreateSubscribeObserver(state, onNext, Stubs<TState>.Throw, Stubs<TState>.Ignore));
    }
}