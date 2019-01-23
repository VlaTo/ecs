using System;
using ClassLibrary1.Core;

namespace ClassLibrary1.Extensions
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="observable"></param>
        /// <param name="onNext"></param>
        /// <param name="onCompleted"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static IDisposable Subscribe<TValue>(this IObservable<TValue> observable, Action<TValue> onNext, Action onCompleted = null, Action<Exception> onError = null)
        {
            if (null == observable)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            if (null == onNext)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            var observer = new SubscribeObserver<TValue>(onNext, onCompleted, onError);

            return observer.Subscribe(observable);
        }
    }
}