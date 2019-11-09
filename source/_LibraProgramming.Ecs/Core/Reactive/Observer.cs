using System;
using System.Threading;
using LibraProgramming.Ecs.Core.Reactive.Operators;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    public static class Observer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onNext"></param>
        /// <returns></returns>
        public static IObserver<T> Create<T>(Action<T> onNext)
        {
            return Create(onNext, Stubs.Throw, Stubs.Nop);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onNext"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError)
        {
            return Create(onNext, onError, Stubs.Nop);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onNext"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IObserver<T> Create<T>(Action<T> onNext, Action onCompleted)
        {
            return Create(onNext, Stubs.Throw, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onNext"></param>
        /// <param name="onError"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (Stubs<T>.Ignore == onNext)
            {
                return new EmptyOnNextAnonymousObserver<T>(onError, onCompleted);
            }

            return new AnonymousObserver<T>(onNext, onError, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <param name="onNext"></param>
        /// <returns></returns>
        public static IObserver<T> Create<T, TState>(TState state, Action<T, TState> onNext)
        {
            return Create(state, onNext, Stubs<TState>.Ignore);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <param name="onNext"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        public static IObserver<T> Create<T, TState>(TState state, Action<T, TState> onNext, Action<TState> onCompleted)
        {
            return CreateSubscribeObserver(state, onNext, Stubs<TState>.Throw, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="observer"></param>
        /// <param name="disposable"></param>
        /// <returns></returns>
        public static IObserver<T> CreateAutoDetachObserver<T>(IObserver<T> observer, IDisposable disposable) =>
            new AutoDetachObserver<T>(observer, disposable);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onNext"></param>
        /// <param name="onError"></param>
        /// <param name="onCompleted"></param>
        /// <returns></returns>
        internal static IObserver<T> CreateSubscribeObserver<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (Stubs<T>.Ignore == onNext)
            {
                return new EmptyOnNextAnonymousObserver<T>(onError, onCompleted);
            }

            return new Subscribe<T>(onNext, onError, onCompleted);
        }

        internal static IObserver<T> CreateSubscribeObserver<T, TState>(TState state, Action<T, TState> onNext, Action<Exception, TState> onError, Action<TState> onCompleted)
        {
            if (Stubs<T, TState>.Ignore == onNext)
            {
                return new EmptyOnNextAnonymousObserver<T, TState>(state, onError, onCompleted);
            }

            return new Subscribe<T, TState>(state, onNext, onError, onCompleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class AnonymousObserver<T> : IObserver<T>
        {
            private readonly Action<T> onNext;
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public AnonymousObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                this.onNext = onNext;
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

            public void OnNext(T value)
            {
                if (0 == stopped)
                {
                    onNext.Invoke(value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class EmptyOnNextAnonymousObserver<T> : IObserver<T>
        {
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public EmptyOnNextAnonymousObserver(Action<Exception> onError, Action onCompleted)
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

            public void OnNext(T value)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class EmptyOnNextAnonymousObserver<T, TState> : IObserver<T>
        {
            private readonly TState state;
            private readonly Action<Exception, TState> onError;
            private readonly Action<TState> onCompleted;
            private int stopped;

            public EmptyOnNextAnonymousObserver(TState state, Action<Exception, TState> onError, Action<TState> onCompleted)
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

            public void OnNext(T value)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class Subscribe<T> : IObserver<T>
        {
            private readonly Action<T> onNext;
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;
            private int stopped;

            public Subscribe(Action<T> onNext, Action<Exception> onError, Action onCompleted)
            {
                this.onNext = onNext;
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

            public void OnNext(T value)
            {
                if (0 == stopped)
                {
                    onNext.Invoke(value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class Subscribe<T, TState> : IObserver<T>
        {
            private readonly TState state;
            private readonly Action<T, TState> onNext;
            private readonly Action<Exception, TState> onError;
            private readonly Action<TState> onCompleted;
            private int stopped;

            public Subscribe(TState state, Action<T, TState> onNext, Action<Exception, TState> onError, Action<TState> onCompleted)
            {
                this.state = state;
                this.onNext = onNext;
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

            public void OnNext(T value)
            {
                if (0 == stopped)
                {
                    onNext.Invoke(value, state);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class Subscribe<T, TState1, TState2> : IObserver<T>
        {
            private readonly TState1 state1;
            private readonly TState2 state2;
            private readonly Action<T, TState1, TState2> onNext;
            private readonly Action<Exception, TState1, TState2> onError;
            private readonly Action<TState1, TState2> onCompleted;
            private int stopped;

            public Subscribe(
                TState1 state1,
                TState2 state2,
                Action<T, TState1, TState2> onNext,
                Action<Exception, TState1, TState2> onError,
                Action<TState1, TState2> onCompleted)
            {
                this.state1 = state1;
                this.state2 = state2;
                this.onNext = onNext;
                this.onError = onError;
                this.onCompleted = onCompleted;
                stopped = 0;
            }

            public void OnCompleted()
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onCompleted.Invoke(state1, state2);
                }
            }

            public void OnError(Exception error)
            {
                if (1 == Interlocked.Increment(ref stopped))
                {
                    onError.Invoke(error, state1, state2);
                }
            }

            public void OnNext(T value)
            {
                if (0 == stopped)
                {
                    onNext.Invoke(value, state1, state2);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private sealed class AutoDetachObserver<T> : OperatorObserverBase<T, T>
        {
            public AutoDetachObserver(IObserver<T> observer, IDisposable disposable)
                : base(observer, disposable)
            {
            }

            public override void OnCompleted()
            {
                try
                {
                    Observer.OnCompleted();
                }
                finally
                {
                    Dispose();
                }
            }

            public override void OnError(Exception error)
            {
                try
                {
                    Observer.OnError(error);
                }
                finally
                {
                    Dispose();
                }
            }

            public override void OnNext(T value)
            {
                try
                {
                    Observer.OnNext(value);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }
        }
    }
}