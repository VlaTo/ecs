using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.Ecs.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TaskCompletionSource
    {
        private readonly TaskCompletionSource<object> tcs;

        /// <summary>
        /// 
        /// </summary>
        public Task Task => tcs.Task;

        /// <summary>
        /// 
        /// </summary>
        public TaskCompletionSource()
        {
            tcs = new TaskCompletionSource<object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public TaskCompletionSource(object state)
        {
            tcs = new TaskCompletionSource<object>(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="creationOptions"></param>
        public TaskCompletionSource(TaskCreationOptions creationOptions)
        {
            tcs = new TaskCompletionSource<object>(creationOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="creationOptions"></param>
        public TaskCompletionSource(object state, TaskCreationOptions creationOptions)
        {
            tcs = new TaskCompletionSource<object>(state, creationOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCanceled() => tcs.SetCanceled();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool TrySetCanceled() => tcs.TrySetCanceled();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool TrySetCanceled(CancellationToken cancellationToken) => tcs.TrySetCanceled(cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public void SetException(Exception exception) => tcs.SetException(exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptions"></param>
        public void SetException(IEnumerable<Exception> exceptions) => tcs.SetException(exceptions);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public bool TrySetException(Exception exception) => tcs.TrySetException(exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public bool TrySetException(IEnumerable<Exception> exceptions) => tcs.TrySetException(exceptions);

        /// <summary>
        /// 
        /// </summary>
        public void SetComplete() => tcs.SetResult(null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool TrySetComplete() => tcs.TrySetResult(null);
    }
}