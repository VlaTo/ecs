using System;
using System.Threading;

namespace ClassLibrary1.Core.Reactive.Collections
{
    /// <summary>
    /// 
    /// </summary>
    internal static class CollectionObserver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <returns></returns>
        public static ICollectionObserver<T> Create<T>(Action<T> onAdded, Action<T> onRemoved)
        {
            return Create(onAdded, onRemoved, Stubs.Throw, Stubs.Nop);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static ICollectionObserver<T> Create<T>(Action<T> onAdded, Action<T> onRemoved, Action onCompleted)
        {
            return Create(onAdded, onRemoved, Stubs.Throw, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static ICollectionObserver<T> Create<T>(Action onCompleted)
        {
            return Create(Stubs<T>.Ignore, Stubs<T>.Ignore, Stubs.Throw, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onError"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static ICollectionObserver<T> Create<T>(Action<T> onAdded, Action<T> onRemoved, Action<Exception> onError, Action onCompleted)
        {
            return CreateSubscribeObserver(onAdded, onRemoved, onError, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <returns></returns>
        public static ICollectionObserver<T> Create<T, TState>(TState state, Action<T, TState> onAdded, Action<T, TState> onRemoved)
        {
            return Create(state, onAdded, onRemoved, Stubs<TState>.Throw, Stubs<TState>.Ignore);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static ICollectionObserver<T> Create<T, TState>(TState state, Action<T, TState> onAdded, Action<T, TState> onRemoved, Action<TState> onCompleted)
        {
            return Create(state, onAdded, onRemoved, Stubs<TState>.Throw, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <param name="onAdded"></param>
        /// <param name="onRemoved"></param>
        /// <param name="onError"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static ICollectionObserver<T> Create<T, TState>(TState state, Action<T, TState> onAdded, Action<T, TState> onRemoved, Action<Exception, TState> onError, Action<TState> onCompleted)
        {
            return CreateSubscribeObserver(state, onAdded, onRemoved, onError, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICollectionObserver<T> Empty<T>() => EmptyCollectionObserver<T>.Instance;

        internal static ICollectionObserver<T> CreateSubscribeObserver<T>(
            Action<T> onAdded,
            Action<T> onRemoved,
            Action<Exception> onError,
            Action onCompleted)
        {
            if (Stubs<T>.Ignore == onAdded && Stubs<T>.Ignore == onRemoved)
            {
                return new EmptyAnonymousCollectionObserver<T>(onError, onCompleted);
            }

            return new AnonymousCollectionObserver<T>(onAdded, onRemoved, onError, onCompleted);
        }

        /*internal static ICollectionObserver<T> CreateSubscribeObserver<T, TState>(
            TState state,
            Action<T, TState> onAdded,
            Action<T, TState> onRemoved,
            Action<TState> onCompleted)
        {
            if (Stubs<T, TState>.Ignore == onAdded && Stubs<T, TState>.Ignore == onRemoved)
            {
                return new EmptyAnonymousCollectionObserver<T, TState>(state, onError, onCompleted);
            }

            return new AnonymousCollectionObserver<T, TState>(state, onAdded, onRemoved, onError, onCompleted);
        }*/

        internal static ICollectionObserver<T> CreateSubscribeObserver<T, TState>(
            TState state,
            Action<T, TState> onAdded,
            Action<T, TState> onRemoved,
            Action<Exception, TState> onError,
            Action<TState> onCompleted)
        {
            if (Stubs<T, TState>.Ignore == onAdded && Stubs<T, TState>.Ignore == onRemoved)
            {
                return new EmptyAnonymousCollectionObserver<T, TState>(state, onError, onCompleted);
            }

            return new AnonymousCollectionObserver<T, TState>(state, onAdded, onRemoved, onError, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private sealed class EmptyCollectionObserver<T> : ICollectionObserver<T>
        {
            public static readonly EmptyCollectionObserver<T> Instance;

            private EmptyCollectionObserver()
            {
            }

            static EmptyCollectionObserver()
            {
                Instance = new EmptyCollectionObserver<T>();
            }

            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnAdded(T item)
            {
            }

            public void OnRemoved(T item)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private sealed class EmptyAnonymousCollectionObserver<T> : ICollectionObserver<T>
        {
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public EmptyAnonymousCollectionObserver(Action<Exception> onError, Action onCompleted)
            {
                this.onError = onError;
                this.onCompleted = onCompleted;
                stopped = 0;
            }

            public void OnCompleted()
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onCompleted.Invoke();
                }
            }

            public void OnError(Exception error)
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onError.Invoke(error);
                }
            }

            public void OnAdded(T item)
            {
            }

            public void OnRemoved(T item)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        private sealed class EmptyAnonymousCollectionObserver<T, TState> : ICollectionObserver<T>
        {
            private readonly TState state;
            private readonly Action<Exception, TState> onError;
            private readonly Action<TState> onCompleted;
            private int stopped;

            public EmptyAnonymousCollectionObserver(TState state, Action<Exception, TState> onError, Action<TState> onCompleted)
            {
                this.state = state;
                this.onError = onError;
                this.onCompleted = onCompleted;
                stopped = 0;
            }

            public void OnCompleted()
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onCompleted.Invoke(state);
                }
            }

            public void OnError(Exception error)
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onError.Invoke(error, state);
                }
            }

            public void OnAdded(T item)
            {
            }

            public void OnRemoved(T item)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private sealed class AnonymousCollectionObserver<T> : ICollectionObserver<T>
        {
            private readonly Action<T> onAdded;
            private readonly Action<T> onRemoved;
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public AnonymousCollectionObserver(
                Action<T> onAdded,
                Action<T> onRemoved,
                Action<Exception> onError,
                Action onCompleted)
            {
                this.onAdded = onAdded;
                this.onRemoved = onRemoved;
                this.onError = onError;
                this.onCompleted = onCompleted;
                stopped = 0;
            }

            public void OnCompleted()
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onCompleted.Invoke();
                }
            }

            public void OnError(Exception error)
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onError.Invoke(error);
                }
            }

            public void OnAdded(T item)
            {
                if (0 == stopped)
                {
                    onAdded.Invoke(item);
                }
            }

            public void OnRemoved(T item)
            {
                if (0 == stopped)
                {
                    onRemoved.Invoke(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        private sealed class AnonymousCollectionObserver<T, TState> : ICollectionObserver<T>
        {
            private readonly TState state;
            private readonly Action<T, TState> onAdded;
            private readonly Action<T, TState> onRemoved;
            private readonly Action<Exception, TState> onError;
            private readonly Action<TState> onCompleted;
            private int stopped;

            public AnonymousCollectionObserver(
                TState state,
                Action<T, TState> onAdded,
                Action<T, TState> onRemoved,
                Action<Exception, TState> onError,
                Action<TState> onCompleted)
            {
                this.state = state;
                this.onAdded = onAdded;
                this.onRemoved = onRemoved;
                this.onError = onError;
                this.onCompleted = onCompleted;
                stopped = 0;
            }

            public void OnCompleted()
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onCompleted.Invoke(state);
                }
            }

            public void OnError(Exception error)
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onError.Invoke(error, state);
                }
            }

            public void OnAdded(T item)
            {
                if (0 == stopped)
                {
                    onAdded.Invoke(item, state);
                }
            }

            public void OnRemoved(T item)
            {
                if (0 == stopped)
                {
                    onRemoved.Invoke(item, state);
                }
            }
        }
    }
}