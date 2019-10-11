using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace LibraProgramming.Ecs
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ComponentState
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = nameof(Alias), Namespace = "http://scheme.ecs.org/elements/entity")]
        public string Alias
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlArray]
        [XmlArrayItem(typeof(PropertyState), ElementName = "Property", Namespace = "http://scheme.ecs.org/elements/entity")]
        public PropertyState[] Properties
        {
            get;
            set;
        }

        public ComponentState()
        {
            Properties = new PropertyState[0];
        }
    }
}