using System;

namespace ClassLibrary1
{
    public interface IObservableCollection<out T>
    {
        IDisposable Subscribe(ICollectionObserver<T> observer);
    }
}