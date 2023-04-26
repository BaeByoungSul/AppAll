using System.Xml.Serialization;

namespace Models.Sap.PP0060;
/// <summary>
/// 기간오픈정보
/// </summary>
public class PP0060
{
    private const string soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
    private const string inf = "http://grpeccpp.esp.com/infesp";

    [XmlRoot(Namespace = soapenv)]
    public class Envelope
    {
        private static XmlSerializerNamespaces staticxmlns;
        public Envelope()
        {
            Header = new EnvHeader();
            Body = new EnvBody();
        }

        [XmlElement(ElementName = "Header")]
        public EnvHeader Header { get; set; }

        [XmlElement(ElementName = "Body")]
        public EnvBody Body { get; set; }

        static Envelope()
        {
            staticxmlns = new XmlSerializerNamespaces();

            staticxmlns.Add("inf", inf);
            staticxmlns.Add("soapenv", soapenv);

        }
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns { get { return staticxmlns; } set { } }
    }

    //[XmlType(Namespace = soapenv)]
    public partial class EnvHeader
    {

    }

    //[XmlType(Namespace = soapenv)]
    public partial class EnvBody
    {
        public EnvBody()
        {
            Req_PP0060 = new Req_PP0060();
        }

        [XmlElement(Namespace = inf, ElementName = "MT_GRP_PP0060_Con")]
        public Req_PP0060 Req_PP0060 { get; set; }
    }
}

public partial class Req_PP0060
{
    public Req_PP0060()
    {
        Header = new RequestHeader();
        Body = new ReqBody_PP0060();
    }
    
    [XmlElement(ElementName = "Header", Namespace = "", Order = 0)]
    public RequestHeader Header { get; set; }

    [XmlElement(ElementName = "Body", Namespace = "", Order = 1)]
    public ReqBody_PP0060 Body { get; set; }



}
public partial class ReqBody_PP0060
{

    [XmlElement(ElementName = "WERKS")]
    public string WERKS { get; set; } = string.Empty;

    [XmlElement(ElementName = "SPMON")]
    public string SPMON { get; set; } = string.Empty;

}


