using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class configurationSystemapplicationHostSitesSite
    {


        public configurationSystemapplicationHostSitesSiteApplication application { get; set; }


        public configurationSystemapplicationHostSitesSiteBindings bindings { get; set; }


        [XmlAttribute()]
        public string name { get; set; }


        [XmlAttribute()]
        public byte id { get; set; }
    }


}
