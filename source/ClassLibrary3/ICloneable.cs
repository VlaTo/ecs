using System;

namespace ClassLibrary3
{
    public interface ICloneable<out T> : ICloneable
    {
        T Clone();
    }
}