using System.Collections.Generic;
using ClassLibrary1.Core;

namespace ClassLibrary1
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