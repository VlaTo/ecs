using System;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IError
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        void OnError(Exception error);
    }
}