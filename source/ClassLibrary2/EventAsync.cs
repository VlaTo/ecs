using System;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventAsync
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscribe"></param>
        /// <param name="cleanup"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task FromEvent(
            Action<EventHandler> subscribe, 
            Action<EventHandler> cleanup,
            Action action = null)
        {
            return new EventHandlerTaskSource(subscribe, cleanup, action).Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscribe"></param>
        /// <param name="cleanup"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task<T> FromEvent<T>(
            Action<EventHandler<T>> subscribe, 
            Action<EventHandler<T>> cleanup,
            Action action = null)
        {
            return new EventHandlerTaskSource<T>(subscribe, cleanup, action).Task;
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class EventHandlerTaskSource
        {
            private readonly TaskCompletionSource<bool> tcs;
            private readonly Action<EventHandler> cleanup;

            public Task Task => tcs.Task;

            public EventHandlerTaskSource(
                Action<EventHandler> subscribe,
                Action<EventHandler> cleanup, 
                Action action)
            {
                if (null == subscribe)
                {
                    throw new ArgumentNullException(nameof(subscribe));
                }

                if (null == cleanup)
                {
	                throw new ArgumentNullException(nameof(cleanup));
                }

                tcs = new TaskCompletionSource<bool>();
                this.cleanup = cleanup;

                subscribe.Invoke(DoComplete);
                action?.Invoke();
            }

            private void DoComplete(object sender, EventArgs e)
            {
                cleanup.Invoke(DoComplete);
                tcs.SetResult(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        private sealed class EventHandlerTaskSource<TEventArgs>
        {
            private readonly TaskCompletionSource<TEventArgs> tcs;
            private readonly Action<EventHandler<TEventArgs>> cleanup;

            public Task<TEventArgs> Task => tcs.Task;

            public EventHandlerTaskSource(
                Action<EventHandler<TEventArgs>> subscribe,
                Action<EventHandler<TEventArgs>> cleanup, 
                Action action)
            {
                if (null == subscribe)
                {
                    throw new Exception();
                }

                if (null == cleanup)
                {
                    throw new Exception();
                }

                tcs = new TaskCompletionSource<TEventArgs>();
                this.cleanup = cleanup;

                subscribe.Invoke(DoComplete);
                action?.Invoke();
            }

            private void DoComplete(object sender, TEventArgs e)
            {
                cleanup.Invoke(DoComplete);
                tcs.SetResult(e);
            }
        }
    }
}