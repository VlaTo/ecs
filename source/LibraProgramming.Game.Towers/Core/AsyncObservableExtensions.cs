using System;
using System.Threading.Tasks;

namespace LibraProgramming.Game.Towers.Core
{
    internal static class AsyncObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IAsyncObservable<T> observable, Func<T, Task> onNext)
        {
            if (null == observable)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            if (null == onNext)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            return observable.Subscribe(new AnonymousAsyncObserver<T>(onNext));
        }

        private class AnonymousAsyncObserver<T> : IAsyncObserver<T>
        {
            private readonly Func<T, Task> onNext;

            public AnonymousAsyncObserver(Func<T, Task> onNext)
            {
                this.onNext = onNext;
            }

            public void OnCompleted()
            {
                ;
            }

            public Task OnNextAsync(T value) => onNext.Invoke(value);

            public void OnError(Exception error)
            {
                throw error;
            }
        }
    }
}