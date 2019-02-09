using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    public class Security
    {
        [XmlElement("access")]
        public SecurityAccess Access { get; set; }

        [XmlElement("authentication")]
        public SecurityAuthentication Authentication { get; set; }
    }
}
