using System;
using System.Collections.Generic;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public interface IComponentEnumerable<out TComponent> : IEnumerable<TComponent>, IDisposable
        where TComponent : IComponent
    {
    }
}