using System;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ComponentAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Alias
        {
            get;
            set;
        }
    }
}