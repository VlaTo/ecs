using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace LibraProgramming.Ecs
{
    [DataContract]
    [XmlRoot(ElementName = "Entity", Namespace = "http://scheme.ecs.org/elements/entity")]
    public sealed class EntityState
    {
        [DataMember, XmlAttribute(AttributeName = nameof(Key), Namespace = "http://scheme.ecs.org/elements/entity")]
        public string Key
        {
            get;
            set;
        }

        [DataMember]
        [XmlAttribute(AttributeName = "Path", Namespace = "http://scheme.ecs.org/elements/entity")]
        [DefaultValue(null)]
        public string EntityPath
        {
            get;
            set;
        }

        [DataMember]
        [XmlAttribute(AttributeName = "Link", Namespace = "http://scheme.ecs.org/elements/entity")]
        [DefaultValue(false)]
        public bool IsReference
        {
            get; 
            set;
        }

        [DataMember, XmlArray]
        [XmlArrayItem(typeof(ComponentState), ElementName = "Component", Namespace = "http://scheme.ecs.org/elements/entity")]
        public ComponentState[] Components
        {
            get;
            set;
        }

        [DataMember, XmlArray]
        [XmlArrayItem(typeof(EntityState), ElementName = "Entity", Namespace = "http://scheme.ecs.org/elements/entity")]
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