using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ClassLibrary1
{
    [DataContract]
    public sealed class EntityState
    {
        [DataMember, XmlAttribute]
        public string Key
        {
            get;
            set;
        }

        [DataMember, XmlAttribute]
        public string EntityPath
        {
            get;
            set;
        }

        [DataMember, XmlArray]
        [XmlArrayItem(typeof(ComponentState))]
        //public IList<ComponentState> Components
        public ComponentState[] Components
        {
            get;
            set;
        }

        [DataMember, XmlArray]
        [XmlArrayItem(typeof(EntityState))]
        //public IList<EntityState> Children
        public EntityState[] Children
        {
            get;
            set;
        }

        public EntityState()
        {
            Components = new ComponentState[0];
            Children = new EntityState[0];
        }
    }
}