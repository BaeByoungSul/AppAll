using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Models.Sap.PP0370;

public class PP0370
{
    private const string soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
    private const string inf = "http://grpeccpp.esp.com/infesp";

    [XmlRoot(Namespace = soapenv)]
    public class Envelope
    {
        private static readonly XmlSerializerNamespaces sapXmlns;

        [XmlElement(ElementName = "Header")]
        public EnvHeader Header { get; set; } = new EnvHeader();

        [XmlElement(ElementName = "Body")]
        public EnvBody Body { get; set; } = new EnvBody();

        static Envelope()
        {
            sapXmlns = new XmlSerializerNamespaces();

            sapXmlns.Add("inf", inf);
            sapXmlns.Add("soapenv", soapenv);

        }
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns { get { return sapXmlns; } set { } }

    }
    public class EnvHeader
    {

    }

    public class EnvBody
    {

        [XmlElement(Namespace = inf, ElementName = "MT_GRP_PP0370_Con")]
        public PP0370_Request Req_PP0370 { get; set; } = new PP0370_Request();

    }

}
