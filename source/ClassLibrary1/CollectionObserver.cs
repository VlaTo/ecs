using System;
using System.Threading;
using ClassLibrary1.Extensions;

namespace ClassLibrary1
{
    public static class CollectionObserver
    {
        public static ICollectionObserver<T> Create<T>(
            Action<T, int> onAdded,
            Action<T, int> onRemoved,
            Action<Exception> onError,
            Action onCompleted)
        {
            if (CollectionStubs<T>.Ignore == onAdded && CollectionStubs<T>.Ignore == onRemoved)
            {
                return new EmptyAnonymousObserver<T>(onError, onCompleted);
            }

            return new AnonymousObserver<T>(onAdded, onRemoved, onError, onCompleted);
        }

        public static ICollectionObserver<T> Create<T, TState>(
            TState state,
            Action<T, TState, int> onAdded,
            Action<T, TState, int> onRemoved,
            Action<Exception, TState> onError,
            Action<TState> onCompleted)
        {
            if (CollectionStubs<T, TState>.Ignore == onAdded && CollectionStubs<T, TState>.Ignore == onRemoved)
            {
                return new EmptyAnonymousObserver<T, TState>(state, onError, onCompleted);
            }

            return new AnonymousObserver<T, TState>(state, onAdded, onRemoved, onError, onCompleted);
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

            public void OnAdded(T item, int index)
            {
            }

            public void OnRemoved(T item, int index)
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

            public void OnAdded(T item, int index)
            {
            }

            public void OnRemoved(T item, int index)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class AnonymousObserver<T> : ICollectionObserver<T>
        {
            private readonly Action<T, int> onAdded;
            private readonly Action<T, int> onRemoved;
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public AnonymousObserver(
                Action<T, int> onAdded,
                Action<T, int> onRemoved,
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

            public void OnAdded(T item, int index)
            {
                if (0 == stopped)
                {
                    onAdded.Invoke(item, index);
                }
            }

            public void OnRemoved(T item, int index)
            {
                if (0 == stopped)
                {
                    onRemoved.Invoke(item, index);
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
            private readonly Action<T, TState, int> onAdded;
            private readonly Action<T, TState, int> onRemoved;
            private readonly Action<Exception, TState> onError;
            private readonly Action<TState> onCompleted;
            private int stopped;

            public AnonymousObserver(
                TState state,
                Action<T, TState, int> onAdded,
                Action<T, TState, int> onRemoved,
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

            public void OnAdded(T item, int index)
            {
                if (0 == stopped)
                {
                    onAdded.Invoke(item, state, index);
                }
            }

            public void OnRemoved(T item, int index)
            {
                if (0 == stopped)
                {
                    onRemoved.Invoke(item, state, index);
                }
            }
        }
    }
}