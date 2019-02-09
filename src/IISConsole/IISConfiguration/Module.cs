using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    public partial class Module
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("lockItem")]
        public bool LockItem { get; set; }
    }

}
