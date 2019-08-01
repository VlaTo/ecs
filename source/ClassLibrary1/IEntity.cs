using System.Collections.Generic;

namespace ClassLibrary1
{
    public interface IEntity
    {
        string Key
        {
            get;
        }

        EntityBase Parent
        {
            get;
        }

        EntityBase Root
        {
            get;
        }

        IEnumerable<IComponent> Components
        {
            get;
        }
    }
}