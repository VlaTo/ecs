using System;
using System.Threading;
using ClassLibrary1.Core.Reactive;
using ClassLibrary1.Extensions;

namespace ClassLibrary1
{
    internal static class EntityObserver
    {
        public static ICollectionObserver<IComponent> Create(
            Action<IComponent> onAdded,
            Action onCompleted)
        {
            if (Stubs<IComponent>.Ignore == onAdded)
            {
                return new EmptyOnAddedAndOnRemovedAnonymousCollectionObserver<IComponent>(Stubs.Throw, onCompleted);
            }

            return Create(onAdded, Stubs<IComponent>.Ignore, onCompleted);
        }

        public static ICollectionObserver<IComponent> Create(
            Action<IComponent> onAdded,
            Action<IComponent> onRemoved)
        {
            if (Stubs<IComponent>.Ignore == onAdded)
            {
                return new EmptyOnAddedAndOnRemovedAnonymousCollectionObserver<IComponent>(Stubs.Throw, Stubs.Nop);
            }

            return Create(onAdded, onRemoved, Stubs.Nop);
        }

        public static ICollectionObserver<IComponent> Create(
            Action<IComponent> onAdded,
            Action<IComponent> onRemoved,
            Action onCompleted)
        {
            if (Stubs<IComponent>.Ignore == onAdded)
            {
                return new EmptyOnAddedAndOnRemovedAnonymousCollectionObserver<IComponent>(Stubs.Throw, onCompleted);
            }

            return CreateSubscribeObserver(onAdded, onRemoved, Stubs.Throw, onCompleted);
        }

        private static ICollectionObserver<IComponent> CreateSubscribeObserver(
            Action<IComponent> onAdded,
            Action<IComponent> onRemoved,
            Action<Exception> onError,
            Action onCompleted)
        {
            if (Stubs<IComponent>.Ignore == onAdded)
            {
                return new EmptyOnAddedAndOnRemovedAnonymousCollectionObserver<IComponent>(onError, onCompleted);
            }

            return new Subscribe<IComponent>(onAdded, onRemoved, onError, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private sealed class EmptyOnAddedAndOnRemovedAnonymousCollectionObserver<T> : ICollectionObserver<T>
        {
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public EmptyOnAddedAndOnRemovedAnonymousCollectionObserver(Action<Exception> onError, Action onCompleted)
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
        private class Subscribe<T> : ICollectionObserver<T>
        {
            private readonly Action<T> onAdded;
            private readonly Action<T> onRemoved;
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public Subscribe(
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
    }
}