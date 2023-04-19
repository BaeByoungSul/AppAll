using System.Xml.Serialization;

namespace Models.Sap;
/// <summary>
/// 생산실적 전송
/// </summary>
public class PP1180
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
            Req_PP1180 = new Req_PP1180();
        }

        [XmlElement(Namespace = inf, ElementName = "MT_GRP_PP1180_Con")]
        public Req_PP1180 Req_PP1180 { get; set; }

    }
}

public  class Req_PP1180
{
    [XmlElement(ElementName = "Header", Namespace = "", Order = 0)]
    public ReqHeader Header { get; set; }

    [XmlElement(ElementName = "Body", Namespace = "", Order = 1)]
    public ReqBody_PP1180 Body { get; set; }
}


public  class ReqBody_PP1180
{
    public ReqBody_PP1180()
    {
        GR_INFOs = new GR_INFO[] { new GR_INFO() };
    }

    [XmlElement(ElementName = "GR_INFO")]
    public GR_INFO[] GR_INFOs { get; set; }

}
public  class GR_INFO
{
    public string WERKS { get; set; } = String.Empty;
    public string AUFNR { get; set; } = String.Empty;
    public string MATNR { get; set; } = String.Empty;
    public string BUDAT { get; set; } = String.Empty;
    public string ZSERNO { get; set; } = String.Empty;
    public string ZJOBNO { get; set; } = String.Empty;
    public string BWART { get; set; } = String.Empty;
    public string ZREAC { get; set; } = String.Empty;
    public string MENGE { get; set; } = String.Empty;
    public string MEINH { get; set; } = String.Empty;
    public string VORNR { get; set; } = String.Empty;
    public string ARBPL { get; set; } = String.Empty;
    public string LMNGA { get; set; } = String.Empty;
    public string XMNGA { get; set; } = String.Empty;
    public string RMNGA { get; set; } = String.Empty;
    public string AUERU { get; set; } = String.Empty;
    public string GRUND { get; set; } = String.Empty;
    public string CHARG { get; set; } = String.Empty;
    public string ZZCHARG { get; set; } = String.Empty;
    public string MJAHR { get; set; } = String.Empty;
    public string MBLNR { get; set; } = String.Empty;
    public string RUECK { get; set; } = String.Empty;
    public string RMZHL { get; set; } = String.Empty;
    public string HSDAT { get; set; } = String.Empty;
    public string LGORT { get; set; } = String.Empty;
    public string CLASS { get; set; } = String.Empty;
    public string ATNAM01 { get; set; } = String.Empty;
    public string ATWRT01 { get; set; } = String.Empty;
    public string ATNAM02 { get; set; } = String.Empty;
    public string ATWRT02 { get; set; } = String.Empty;
    public string ATNAM03 { get; set; } = String.Empty;
    public string ATWRT03 { get; set; } = String.Empty;
    public string ATNAM04 { get; set; } = String.Empty;
    public string ATWRT04 { get; set; } = String.Empty;
    public string ATNAM05 { get; set; } = String.Empty;
    public string ATWRT05 { get; set; } = String.Empty;
    public string ATNAM06 { get; set; } = String.Empty;
    public string ATWRT06 { get; set; } = String.Empty;
    public string ATNAM07 { get; set; } = String.Empty;
    public string ATWRT07 { get; set; } = String.Empty;
    public string ATNAM08 { get; set; } = String.Empty;
    public string ATWRT08 { get; set; } = String.Empty;
    public string ATNAM09 { get; set; } = String.Empty;
    public string ATWRT09 { get; set; } = String.Empty;
    public string ATNAM10 { get; set; } = String.Empty;
    public string ATWRT10 { get; set; } = String.Empty;
    public string ATNAM11 { get; set; } = String.Empty;
    public string ATWRT11 { get; set; } = String.Empty;
    public string ATNAM12 { get; set; } = String.Empty;
    public string ATWRT12 { get; set; } = String.Empty;
    public string ATNAM13 { get; set; } = String.Empty;
    public string ATWRT13 { get; set; } = String.Empty;
    public string ATNAM14 { get; set; } = String.Empty;
    public string ATWRT14 { get; set; } = String.Empty;
    public string ATNAM15 { get; set; } = String.Empty;
    public string ATWRT15 { get; set; } = String.Empty;
    public string ATNAM16 { get; set; } = String.Empty;
    public string ATWRT16 { get; set; } = String.Empty;
    public string ATNAM17 { get; set; } = String.Empty;
    public string ATWRT17 { get; set; } = String.Empty;
    public string ATNAM18 { get; set; } = String.Empty;
    public string ATWRT18 { get; set; } = String.Empty;
    public string ATNAM19 { get; set; } = String.Empty;
    public string ATWRT19 { get; set; } = String.Empty;
    public string ATNAM20 { get; set; } = String.Empty;
    public string ATWRT20 { get; set; } = String.Empty;
    public string ATNAM21 { get; set; } = String.Empty;
    public string ATWRT21 { get; set; } = String.Empty;
    public string ATNAM22 { get; set; } = String.Empty;
    public string ATWRT22 { get; set; } = String.Empty;
    public string ATNAM23 { get; set; } = String.Empty;
    public string ATWRT23 { get; set; } = String.Empty;
    public string ATNAM24 { get; set; } = String.Empty;
    public string ATWRT24 { get; set; } = String.Empty;

}
