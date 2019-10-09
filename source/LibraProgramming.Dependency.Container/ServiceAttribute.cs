using System;

namespace LibraProgramming.Dependency.Container
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ServiceAttribute : Attribute
    {
        public string Key
        {
            get;
            set;
        }
    }
}