using System;

namespace ClassLibrary1.Core.Reactive
{
    public static partial class ObserverExtensions
    {
        public static IObserver<T> Synchronize<T>(this IObserver<T> observer) =>
            new SynchronizedObserver<T>(observer, new object());

        public static IObserver<T> Synchronize<T>(this IObserver<T> observer, object gate) =>
            new SynchronizedObserver<T>(observer, gate);
    }
}