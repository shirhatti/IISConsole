using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class WebServer
    {

        [XmlArray("globalModules")]
        [XmlArrayItem("add", IsNullable = false)]
        public GlobalModule[] globalModules { get; set; }

        [XmlElement("handlers")]
        public HandlersSection HandlersSection { get; set; }


        [XmlArray("modules")]
        [XmlArrayItem("add", IsNullable = false)]
        public Module[] Modules { get; set; }

        [XmlElement("security")]
        public Security Security { get; set; }

    }


}
