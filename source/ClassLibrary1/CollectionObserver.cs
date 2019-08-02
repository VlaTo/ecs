using System;
using System.Threading;
using ClassLibrary1.Core.Reactive;

namespace ClassLibrary1
{
    public static class CollectionObserver
    {
        public static ICollectionObserver<T> Create<T>(Action<T> onAdded, Action<T> onRemoved)
        {
            return Create(onAdded, onRemoved, Stubs.Throw, Stubs.Nop);
        }

        public static ICollectionObserver<T> Create<T>(Action<T> onAdded, Action<T> onRemoved, Action onCompleted)
        {
            return Create(onAdded, onRemoved, Stubs.Throw, onCompleted);
        }

        public static ICollectionObserver<T> Create<T>(Action onCompleted)
        {
            return Create(Stubs<T>.Ignore, Stubs<T>.Ignore, Stubs.Throw, onCompleted);
        }

        public static ICollectionObserver<T> Create<T>(Action<T> onAdded, Action<T> onRemoved, Action<Exception> onError, Action onCompleted)
        {
            if (Stubs<T>.Ignore == onAdded && Stubs<T>.Ignore == onRemoved)
            {
                return new EmptyAnonymousObserver<T>(onError, onCompleted);
            }

            return new AnonymousObserver<T>(onAdded, onRemoved, onError, onCompleted);
        }

        public static ICollectionObserver<T> Create<T, TState>(TState state, Action<T, TState> onAdded, Action<T, TState> onRemoved)
        {
            return Create(state, onAdded, onRemoved, Stubs<TState>.Throw, Stubs<TState>.Ignore);
        }

        public static ICollectionObserver<T> Create<T, TState>(TState state, Action<T, TState> onAdded, Action<T, TState> onRemoved, Action<Exception, TState> onError, Action<TState> onCompleted)
        {
            if (Stubs<T, TState>.Ignore == onAdded && Stubs<T, TState>.Ignore == onRemoved)
            {
                return new EmptyAnonymousObserver<T, TState>(state, onError, onCompleted);
            }

            return new AnonymousObserver<T, TState>(state, onAdded, onRemoved, onError, onCompleted);
        }

        public static ICollectionObserver<T> Empty<T>()
        {
            return EmptyObserver<T>.Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class EmptyObserver<T> : ICollectionObserver<T>
        {
            public static readonly EmptyObserver<T> Instance;

            private EmptyObserver()
            {
            }

            static EmptyObserver()
            {
                Instance = new EmptyObserver<T>();
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
        private class EmptyAnonymousObserver<T> : ICollectionObserver<T>
        {
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public EmptyAnonymousObserver(Action<Exception> onError, Action onCompleted)
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
        private class EmptyAnonymousObserver<T, TState> : ICollectionObserver<T>
        {
            private readonly TState state;
            private readonly Action<Exception, TState> onError;
            private readonly Action<TState> onCompleted;
            private int stopped;

            public EmptyAnonymousObserver(TState state, Action<Exception, TState> onError, Action<TState> onCompleted)
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
        private class AnonymousObserver<T> : ICollectionObserver<T>
        {
            private readonly Action<T> onAdded;
            private readonly Action<T> onRemoved;
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public AnonymousObserver(
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
        private class AnonymousObserver<T, TState> : ICollectionObserver<T>
        {
            private readonly TState state;
            private readonly Action<T, TState> onAdded;
            private readonly Action<T, TState> onRemoved;
            private readonly Action<Exception, TState> onError;
            private readonly Action<TState> onCompleted;
            private int stopped;

            public AnonymousObserver(
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