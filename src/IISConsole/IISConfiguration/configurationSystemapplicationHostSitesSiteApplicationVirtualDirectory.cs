using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class configurationSystemapplicationHostSitesSiteApplicationVirtualDirectory
    {


        [XmlAttribute()]
        public string path { get; set; }


        [XmlAttribute()]
        public string physicalPath { get; set; }
    }


}
