using System;
using System.Threading.Tasks;

namespace LibraProgramming.Game.Towers.Core
{
    public interface IAsyncObserver<in T>
    {
        void OnCompleted();

        Task OnNextAsync(T value);

        void OnError(Exception error);
    }

    public interface IAsyncObservable<out T>
    {
        IDisposable Subscribe(IAsyncObserver<T> observer);
    }
}