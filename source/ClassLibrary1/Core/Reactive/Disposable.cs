using System;

namespace ClassLibrary1.Core.Reactive
{
    public static class Disposable
    {
        public static readonly IDisposable Empty = EmptyDisposable.Instance;

        public static IDisposable Create(Action disposeAction) => new AnonymousDisposable(disposeAction);

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