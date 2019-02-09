using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class configurationSystemapplicationHostSitesSiteBindingsBinding
    {


        [XmlAttribute()]
        public string protocol { get; set; }


        [XmlAttribute()]
        public string bindingInformation { get; set; }
    }


}
