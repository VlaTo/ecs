using System;

namespace ClassLibrary1
{
    public interface ISubject<T> : IObservable<T>, IObserver<T>, IDisposable
    {
    }
}