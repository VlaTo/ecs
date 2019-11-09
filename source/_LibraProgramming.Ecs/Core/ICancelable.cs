using System;

namespace LibraProgramming.Ecs.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICancelable : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsDisposed
        {
            get;
        }
    }
}