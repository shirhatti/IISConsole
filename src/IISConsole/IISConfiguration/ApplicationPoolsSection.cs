using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    public class ApplicationPoolsSection
    {
        [XmlElement("add")]
        public ApplicationPool[] ApplicationPools { get; set; }
    }
}
