using ClassLibrary1.Core;
using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Subject<T> : ObservableBase<T>, ISubject<T>
    {
        /// <inheritdoc cref="ObservableBase{T}.Subscribe" />
        public override IDisposable Subscribe(IObserver<T> observer)
        {
            var subscription = base.Subscribe(observer);

            return subscription;
        }

        void IObserver<T>.OnCompleted()
        {
            Completed();
        }

        void IObserver<T>.OnError(Exception error)
        {
            Error(error);
        }

        void IObserver<T>.OnNext(T value)
        {
            Next(value);
        }

        void IDisposable.Dispose()
        {
            Release();
        }
    }
}