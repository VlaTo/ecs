using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    public static class Disposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IDisposable Empty = EmptyDisposable.Instance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposeAction"></param>
        /// <returns></returns>
        public static IDisposable Create(Action disposeAction) => new AnonymousDisposable(disposeAction);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <param name="disposeAction"></param>
        /// <returns></returns>
        public static IDisposable CreateWithState<TState>(TState state, Action<TState> disposeAction) =>
            new AnonymousDisposable<TState>(state, disposeAction);

        /// <summary>
        /// 
        /// </summary>
        private class EmptyDisposable : IDisposable
        {
            public static readonly EmptyDisposable Instance;

            private EmptyDisposable()
            {
            }

            static EmptyDisposable()
            {
                Instance = new EmptyDisposable();
            }

            public void Dispose()
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class AnonymousDisposable : IDisposable
        {
            private bool isDisposed;
            private readonly Action dispose;

            public AnonymousDisposable(Action dispose)
            {
                this.dispose = dispose;
            }

            public void Dispose()
            {
                if (false == isDisposed)
                {
                    isDisposed = true;
                    dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class AnonymousDisposable<T> : IDisposable
        {
            private bool isDisposed;
            private readonly T state;
            private readonly Action<T> dispose;

            public AnonymousDisposable(T state, Action<T> dispose)
            {
                this.state = state;
                this.dispose = dispose;
            }

            public void Dispose()
            {
                if (false == isDisposed)
                {
                    isDisposed = true;
                    dispose(state);
                }
            }
        }
    }
}