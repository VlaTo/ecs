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

        public IList<ComponentState> Components
        {
            get;
        }

        public EntityState()
        {
            Components = new List<ComponentState>();
        }
    }
}