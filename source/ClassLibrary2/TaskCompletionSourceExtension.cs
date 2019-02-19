using System;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    /// <summary>
    /// 
    /// </summary>
    public static class TaskCompletionSourceExtension
    {
        /// <summary>
        /// Creates a new TCS for use with async code, and which forces its continuations to execute asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the TCS.</typeparam>
        public static TaskCompletionSource<TResult> CreateAsyncTaskSource<TResult>()
        {
            return new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TSourceResult"></typeparam>
        /// <param name="this"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public static bool TryCompleteFromTask<TResult, TSourceResult>(this TaskCompletionSource<TResult> @this, Task<TSourceResult> task)
            where TSourceResult : TResult
        {
            if (task.IsFaulted)
            {
                return @this.TrySetException(task.Exception?.InnerExceptions);
            }

            if (task.IsCanceled)
            {
                //return @this.TrySetCanceled();
                try
                {
                    task.WaitAndUnwrapException();
                }
                catch (OperationCanceledException exception)
                {
                    var token = exception.CancellationToken;
                    return token.IsCancellationRequested ? @this.TrySetCanceled(token) : @this.TrySetCanceled();
                }
            }

            return @this.TrySetResult(task.Result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public static bool TryCompleteFromTask(this TaskCompletionSource @this, Task task)
        {
            if (task.IsFaulted)
            {
                return @this.TryFail(task.Exception?.InnerExceptions);
            }

            if (task.IsCanceled)
            {
                //return @this.TryCancel();
                try
                {
                    task.WaitAndUnwrapException();
                }
                catch (OperationCanceledException exception)
                {
                    var token = exception.CancellationToken;
                    return token.IsCancellationRequested ? @this.TryCancel(token) : @this.TryCancel();
                }
            }

            return @this.TryComplete();
        }
    }
}