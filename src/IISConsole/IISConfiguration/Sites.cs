using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class Sites
    {


        public configurationSystemapplicationHostSitesSite site { get; set; }


        public configurationSystemapplicationHostSitesApplicationDefaults applicationDefaults { get; set; }


        public configurationSystemapplicationHostSitesVirtualDirectoryDefaults virtualDirectoryDefaults { get; set; }
    }


}
