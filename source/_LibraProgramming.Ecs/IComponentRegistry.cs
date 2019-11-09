using System;

namespace LibraProgramming.Ecs
{
    public interface IComponentRegistry
    {
        void Add(string alias, Func<IComponent> create);
    }
}