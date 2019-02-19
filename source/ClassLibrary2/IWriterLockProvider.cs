using System;
using System.Threading;

namespace ClassLibrary2
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWriterLockProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> AccquireWriterLockAsync(CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        AwaitableDisposable<IDisposable> AccquireWriterLockAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        IDisposable AccquireWriterLock(CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDisposable AccquireWriterLock();
    }
}