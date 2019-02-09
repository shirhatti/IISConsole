using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    public class HandlersSection
    {
        [XmlElement("add")]
        public Handler[] Handlers { get; set; }

        [XmlAttribute("accessPolicy")]
        public string AccessPolicy { get; set; }
    }
}
