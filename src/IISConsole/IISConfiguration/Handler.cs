using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class Handler
    {


        [XmlAttribute()]
        public string name { get; set; }


        [XmlAttribute()]
        public string path { get; set; }


        [XmlAttribute()]
        public string verb { get; set; }


        [XmlAttribute()]
        public string modules { get; set; }


        [XmlAttribute()]
        public string resourceType { get; set; }
        [XmlAttribute()]
        public string requireAccess { get; set; }
    }


}
