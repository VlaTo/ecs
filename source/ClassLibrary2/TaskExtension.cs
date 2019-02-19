using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    /// <summary>
    /// 
    /// </summary>
    public static class TaskExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        public static void WaitAndUnwrapException(this Task @this)
        {
            @this.GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="ct"></param>
        public static void WaitAndUnwrapException(this Task @this, CancellationToken ct)
        {
            try
            {
                @this.Wait(ct);
            }
            catch (AggregateException exception)
            {
                throw ExceptionHelper.PrepareForRethrow(exception.InnerException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> @this)
        {
            return @this.GetAwaiter().GetResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> @this, CancellationToken ct)
        {
            try
            {
                @this.Wait(ct);
                return @this.Result;
            }
            catch (AggregateException exception)
            {
                throw ExceptionHelper.PrepareForRethrow(exception.InnerException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        public static void WaitWithoutException(this Task @this)
        {
            if (@this.IsCompleted)
            {
                return;
            }

            var result = (IAsyncResult) @this;

            result.AsyncWaitHandle.WaitOne();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="ct"></param>
        public static void WaitWithoutException(this Task @this, CancellationToken ct)
        {
            if (@this.IsCompleted)
            {
                return;
            }

            ct.ThrowIfCancellationRequested();

            var index = WaitHandle.WaitAny(new[]
            {
                ((IAsyncResult) @this).AsyncWaitHandle, ct.WaitHandle
            });

            if (0 != index)
            {
                ct.ThrowIfCancellationRequested();
            }
        }
    }
}