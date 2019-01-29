using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ClassLibrary1
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
    }
}