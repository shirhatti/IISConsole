using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole.IISConfiguration
{
    public class SecurityAuthentication
    {
        [XmlElement("anonymousAuthentication")]
        public AnonymousAuthentication AnonymousAuthentication { get; set; }
    }
}
