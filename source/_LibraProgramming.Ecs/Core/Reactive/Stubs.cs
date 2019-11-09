using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Stubs
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Action Nop = () => { };

        /// <summary>
        /// 
        /// </summary>
        public static readonly Action<Exception> Throw = exception => throw exception;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static IObservable<TSource> CatchIgnore<TSource>(Exception exception) => Observable.Empty<TSource>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class Stubs<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Action<T> Ignore = value => { };

        /// <summary>
        /// 
        /// </summary>
        public static readonly Func<T, T> Identity = value => value;

        /// <summary>
        /// 
        /// </summary>
        public static readonly Action<Exception, T> Throw = (exception, _) => throw exception;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    internal static class Stubs<T1, T2>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Action<T1, T2> Ignore = (x, y) => { };

        /// <summary>
        /// 
        /// </summary>
        public static readonly Action<Exception, T1, T2> Throw = (exception, _, __) => throw exception;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    internal static class Stubs<T1, T2, T3>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Action<T1, T2, T3> Ignore = (x, y, z) => { };

        /// <summary>
        /// 
        /// </summary>
        public static readonly Action<Exception, T1, T2, T3> Throw = (exception, _, __, ___) => throw exception;
    }
}