using System.Collections.Generic;
using LibraProgramming.Ecs.Core;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        string Key
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        EntityBase Parent
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        EntityBase Root
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IComponent> Components
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        IEntityCollection Children
        {
            get;
        }
    }
}