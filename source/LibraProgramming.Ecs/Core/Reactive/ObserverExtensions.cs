using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ObserverExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observer"></param>
        /// <returns></returns>
        public static IObserver<T> Synchronize<T>(this IObserver<T> observer) =>
            new SynchronizedObserver<T>(observer, new object());

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observer"></param>
        /// <param name="gate"></param>
        /// <returns></returns>
        public static IObserver<T> Synchronize<T>(this IObserver<T> observer, object gate) =>
            new SynchronizedObserver<T>(observer, gate);
    }
}