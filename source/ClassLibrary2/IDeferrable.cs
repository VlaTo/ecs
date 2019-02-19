using System;

namespace ClassLibrary2
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDeferrable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDisposable GetDeferral();
    }
}