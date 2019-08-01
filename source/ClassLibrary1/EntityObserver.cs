using System;
using System.Threading;
using ClassLibrary1.Core.Reactive;
using ClassLibrary1.Extensions;

namespace ClassLibrary1
{
    internal static class EntityObserver
    {
        public static ICollectionObserver<IComponent> Create(
            Action<IComponent, int> onAdded,
            Action onCompleted)
        {
            if (CollectionStubs<IComponent>.Ignore == onAdded)
            {
                return new EmptyOnAddedAndOnRemovedAnonymousCollectionObserver<IComponent>(Stubs.Throw, onCompleted);
            }

            return Create(onAdded, CollectionStubs<IComponent>.Ignore, onCompleted);
        }

        public static ICollectionObserver<IComponent> Create(
            Action<IComponent, int> onAdded,
            Action<IComponent, int> onRemoved)
        {
            if (CollectionStubs<IComponent>.Ignore == onAdded)
            {
                return new EmptyOnAddedAndOnRemovedAnonymousCollectionObserver<IComponent>(Stubs.Throw, Stubs.Nop);
            }

            return Create(onAdded, onRemoved, Stubs.Nop);
        }

        public static ICollectionObserver<IComponent> Create(
            Action<IComponent, int> onAdded,
            Action<IComponent, int> onRemoved,
            Action onCompleted)
        {
            if (CollectionStubs<IComponent>.Ignore == onAdded)
            {
                return new EmptyOnAddedAndOnRemovedAnonymousCollectionObserver<IComponent>(Stubs.Throw, onCompleted);
            }

            return CreateSubscribeObserver(onAdded, onRemoved, Stubs.Throw, onCompleted);
        }

        private static ICollectionObserver<IComponent> CreateSubscribeObserver(
            Action<IComponent, int> onAdded,
            Action<IComponent, int> onRemoved,
            Action<Exception> onError,
            Action onCompleted)
        {
            if (CollectionStubs<IComponent>.Ignore == onAdded)
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
        private class Subscribe<T> : ICollectionObserver<T>
        {
            private readonly Action<T, int> onAdded;
            private readonly Action<T, int> onRemoved;
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public Subscribe(
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
    }
}