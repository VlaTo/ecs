using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ClassLibrary1
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
        [DataMember, XmlAttribute]
        public string Alias
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember, XmlArray]
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