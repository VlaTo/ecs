﻿using System.Threading.Tasks;
using LibraProgramming.Ecs.Core;

namespace LibraProgramming.Ecs.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TaskCompletionSourceExtension
    {
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
                return @this.TrySetException(task.Exception.InnerExceptions);
            }

            if (task.IsCanceled)
            {
                return @this.TrySetCanceled();
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
                return @this.TrySetException(task.Exception.InnerExceptions);
            }

            if (task.IsCanceled)
            {
                return @this.TrySetCanceled();
            }

            return @this.TrySetComplete();
        }

        /*public static bool TryCompleteFromEventArgs<TResult>(this TaskCompletionSource<TResult> @this, Task task)
        {
            
        }*/
    }
}