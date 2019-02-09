using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class configurationSystemapplicationHostSitesSiteBindings
    {


        public configurationSystemapplicationHostSitesSiteBindingsBinding binding { get; set; }
    }


}
