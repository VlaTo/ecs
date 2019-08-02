using System;
using ClassLibrary1.Core.Reactive;

namespace ClassLibrary1.Extensions
{
    internal static class ObservableCollectionExtensions
    {
        public static IDisposable Subscribe<T>(this IObservableCollection<T> source, Action<T> onAdded) =>
            source.Subscribe(CollectionObserver.Create(onAdded, Stubs<T>.Ignore, Stubs.Throw, Stubs.Nop));

        public static IDisposable Subscribe<T>(this IObservableCollection<T> source, Action<T> onAdded, Action<T> onRemoved) =>
            source.Subscribe(CollectionObserver.Create(onAdded, onRemoved, Stubs.Throw, Stubs.Nop));

        public static IDisposable Subscribe<T>(this IObservableCollection<T> source, Action<T> onAdded, Action<T> onRemoved, Action onCompleted) =>
            source.Subscribe(CollectionObserver.Create(onAdded, onRemoved, Stubs.Throw, onCompleted));

        public static IDisposable Subscribe<T, TState>(this IObservableCollection<T> source, TState state, Action<T, TState> onAdded, Action<T, TState> onRemoved) =>
            source.Subscribe(CollectionObserver.Create(state, onAdded, onRemoved, Stubs<TState>.Throw, Stubs<TState>.Ignore));

        public static IDisposable Subscribe<T, TState>(this IObservableCollection<T> source, TState state, Action<T, TState> onAdded, Action<T, TState> onRemoved, Action<TState> onCompleted) =>
            source.Subscribe(CollectionObserver.Create(state, onAdded, onRemoved, Stubs<TState>.Throw, onCompleted));
    }
}