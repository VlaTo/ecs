using System;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public EntityException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public EntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}