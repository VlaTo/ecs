using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace LibraProgramming.Ecs
{
    [DataContract]
    public sealed class PropertyState
    {
        [DataMember, XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        [DataMember, XmlText]
        public string Value
        {
            get;
            set;
        }

        public PropertyState()
        {
        }

        public PropertyState(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}