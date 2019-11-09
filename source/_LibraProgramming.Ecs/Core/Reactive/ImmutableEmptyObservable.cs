using System;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ImmutableEmptyObservable<T> : IObservable<T>
    {
        internal static readonly ImmutableEmptyObservable<T> Instance;

        /// <summary>
        /// 
        /// </summary>
        public bool IsRequiredSubscribeOnCurrentThread => false;

        private ImmutableEmptyObservable()
        {
        }

        static ImmutableEmptyObservable()
        {
            Instance = new ImmutableEmptyObservable<T>();
        }

        /// <inheritdoc cref="IObservable{T}.Subscribe" />
        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}