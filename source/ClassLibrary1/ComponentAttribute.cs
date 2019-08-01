using System;

namespace ClassLibrary1
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ComponentAttribute : Attribute
    {
        public string Alias
        {
            get;
            set;
        }
    }
}