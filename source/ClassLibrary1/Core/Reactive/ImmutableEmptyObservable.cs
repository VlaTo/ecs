using System;

namespace ClassLibrary1.Core.Reactive
{
    internal class ImmutableEmptyObservable<T> : IObservable<T>
    {
        internal static readonly ImmutableEmptyObservable<T> Instance;

        public bool IsRequiredSubscribeOnCurrentThread => false;

        private ImmutableEmptyObservable()
        {
        }

        static ImmutableEmptyObservable()
        {
            Instance = new ImmutableEmptyObservable<T>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}