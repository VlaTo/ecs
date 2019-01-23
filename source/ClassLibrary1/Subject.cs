using System;

namespace ClassLibrary1
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Subject<T> : ObservableBase<T>, ISubject<T>
    {
        public override IDisposable Subscribe(IObserver<T> observer)
        {
            var subscription = base.Subscribe(observer);

            return subscription;
        }

        public void OnCompleted()
        {
            Completed();
        }

        public void OnError(Exception error)
        {
            Error(error);
        }

        public void OnNext(T value)
        {
            Next(value);
        }

        public void Dispose()
        {
            Release();
        }
    }
}