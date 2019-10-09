using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public interface IComponentEnumerable<out TComponent> : IEnumerable<TComponent>, IDisposable
        where TComponent : IComponent
    {
    }
}