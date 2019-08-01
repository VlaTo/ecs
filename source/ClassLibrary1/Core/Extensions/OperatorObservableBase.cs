using System;
using ClassLibrary1.Core.Reactive;
using ClassLibrary1.Core.Reactive.Schedulers;

namespace ClassLibrary1.Core.Extensions
{
    public abstract class OperatorObservableBase<T> : IObservable<T>
    {
        private readonly bool requireSubscribeOnCurrentThread;

        public bool IsRequireSubscribeOnCurrentThread => requireSubscribeOnCurrentThread;

        protected OperatorObservableBase(bool requireSubscribeOnCurrentThread)
        {
            this.requireSubscribeOnCurrentThread = requireSubscribeOnCurrentThread;
        }

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

        protected abstract IDisposable SubscribeCore(IObserver<T> observer, IDisposable disposable);
    }
}