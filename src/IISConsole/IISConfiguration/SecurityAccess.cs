using System;
using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    public class SecurityAccess
    {
        [XmlAttribute("sslFlags")]
        public string SslFlags { get; set; }
    }
}
