using System.Collections.Generic;

namespace ClassLibrary1
{
    public sealed class EntityState
    {
        public string Key
        {
            get;
            set;
        }

        public string EntityPath
        {
            get;
            set;
        }

        public IList<ComponentState> Components
        {
            get;
        }

        public IList<EntityState> Children
        {
            get;
        }

        public EntityState()
        {
            Components = new List<ComponentState>();
            Children = new List<EntityState>();
        }
    }
}