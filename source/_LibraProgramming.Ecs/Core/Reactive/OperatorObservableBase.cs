using System;
using LibraProgramming.Ecs.Core.Reactive.Schedulers;

namespace LibraProgramming.Ecs.Core.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class OperatorObservableBase<T> : IObservable<T>
    {
        private readonly bool requireSubscribeOnCurrentThread;

        /// <summary>
        /// 
        /// </summary>
        public bool IsRequireSubscribeOnCurrentThread => requireSubscribeOnCurrentThread;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requireSubscribeOnCurrentThread"></param>
        protected OperatorObservableBase(bool requireSubscribeOnCurrentThread)
        {
            this.requireSubscribeOnCurrentThread = requireSubscribeOnCurrentThread;
        }

        /// <inheritdoc cref="IObservable{T}.Subscribe" />
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var subscription = new SingleAssignmentDisposable();

            if (requireSubscribeOnCurrentThread && Scheduler.IsCurrentThreadSchedulerScheduleRequired)
            {
                Scheduler.CurrentThread.Schedule(() => subscription.Disposable = SubscribeCore(observer, subscription));
            }
            else
            {
                subscription.Disposable = SubscribeCore(observer, subscription);
            }

            return subscription;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="disposable"></param>
        /// <returns></returns>
        protected abstract IDisposable SubscribeCore(IObserver<T> observer, IDisposable disposable);
    }
}