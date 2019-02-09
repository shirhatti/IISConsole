using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    [Serializable]
    [XmlType("configuration")]
    [XmlRoot(Namespace = "")]
    public partial class Configuration
    {
        //[XmlArrayItem("sectionGroup", IsNullable = false)]
        //public configurationSectionGroup[] configSections { get; set; }

        [XmlElement("system.applicationHost")]
        public ApplicationHost ApplicationHost { get; set; }

        [XmlElement("system.webServer")]
        public WebServer WebServer { get; set; }
    }


}
