using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace IISConsole
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class configurationSystemwebServerAspNetCore
    {

        [XmlAttribute()]
        public string processPath { get; set; }


        [XmlAttribute()]
        public string arguments { get; set; }


        [XmlAttribute()]
        public bool stdoutLogEnabled { get; set; }


        [XmlAttribute()]
        public string stdoutLogFile { get; set; }
    }


}
