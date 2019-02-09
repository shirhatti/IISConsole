using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    public partial class ApplicationHost
    {
        [XmlElement("applicationPools")]
        public ApplicationPoolsSection ApplicationPools { get; set; }

        public Sites Sites { get; set; }
    }
}
