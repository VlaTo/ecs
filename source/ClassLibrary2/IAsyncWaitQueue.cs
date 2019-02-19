using System;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncWaitQueue<T>
    {
        /// <summary>
        /// 
        /// </summary>
         bool IsEmpty
         {
             get;
         }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<T> EnqueueAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        IDisposable Dequeue(T result = default(T));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        IDisposable DequeueAll(T result = default(T));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        IDisposable TryCancel(Task task);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDisposable CancelAll();
    }
}