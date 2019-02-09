using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    public class ApplicationPool
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
