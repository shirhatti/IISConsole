using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    public class AnonymousAuthentication
    {
        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }


        [XmlAttribute("userName")]
        public string UserName { get; set; }
    }
}
