using System;

namespace ClassLibrary1.Core.Reactive
{
    internal static class Stubs
    {
        public static readonly Action Nop = () => { };

        public static readonly Action<Exception> Throw = exception => throw exception;

        public static IObservable<TSource> CatchIgnore<TSource>(Exception exception) => Observable.Empty<TSource>();
    }

    internal static class Stubs<T>
    {
        public static readonly Action<T> Ignore = value => { };

        public static readonly Func<T, T> Identity = value => value;

        public static readonly Action<Exception, T> Throw = (exception, _) => throw exception;
    }

    internal static class Stubs<T1, T2>
    {
        public static readonly Action<T1, T2> Ignore = (x, y) => { };

        public static readonly Action<Exception, T1, T2> Throw = (exception, _, __) => throw exception;
    }

    internal static class Stubs<T1, T2, T3>
    {
        public static readonly Action<T1, T2, T3> Ignore = (x, y, z) => { };

        public static readonly Action<Exception, T1, T2, T3> Throw = (exception, _, __, ___) => throw exception;
    }
}