using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class configurationSystemapplicationHostSitesSiteApplication
    {

        private configurationSystemapplicationHostSitesSiteApplicationVirtualDirectory virtualDirectoryField;


        public configurationSystemapplicationHostSitesSiteApplicationVirtualDirectory virtualDirectory
        {
            get
            {
                return this.virtualDirectoryField;
            }
            set
            {
                this.virtualDirectoryField = value;
            }
        }


        [XmlAttribute()]
        public string path { get; set; }
    }


}
