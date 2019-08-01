using System;
using ClassLibrary1.Core.Extensions;

namespace ClassLibrary1.Core.Reactive
{
    public class ThrowObserver<T> : IObserver<T>
    {
        public static readonly ThrowObserver<T> Instance;

        private ThrowObserver()
        {

        }

        static ThrowObserver()
        {
            Instance = new ThrowObserver<T>();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            error.Throw();
        }

        public void OnNext(T value)
        {
        }
    }
}