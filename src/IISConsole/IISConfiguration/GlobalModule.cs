using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class GlobalModule
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("image")]
        public string Image { get; set; }
    }


}
