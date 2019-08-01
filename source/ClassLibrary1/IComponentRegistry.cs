using System;

namespace ClassLibrary1
{
    public interface IComponentRegistry
    {
        void Add(string alias, Func<IComponent> create);
    }
}