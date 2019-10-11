using System;

namespace LibraProgramming.Ecs.Core
{
    internal sealed class ComponentResolver
    {
        public ComponentResolver(Func<IComponent> func)
        {
        }

        public IComponent GetComponent()
        {
            throw new NotImplementedException();
        }
    }
}