using System;

namespace ClassLibrary1.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISubject<T> : IObservable<T>, IObserver<T>, IDisposable
    {
    }
}