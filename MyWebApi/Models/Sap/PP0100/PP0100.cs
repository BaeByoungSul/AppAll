// http://infvpidb01.kolon.com:50000/dir/wsdl?p=ic/6011ce259e983024855fdbaa8af28b61
// http://infvpidb01.kolon.com:50000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_QAS&receiverParty=&receiverService=&interface=SI_GRP_PP0100_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp


using System.Xml.Serialization;
using Models.Sap;

namespace MyWebApi.Models.Sap.PP0100;

/// <summary>
/// 생산투입
/// </summary>
public class PP0100
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
            Req_PP0100 = new Req_PP0100();
        }

        [XmlElement(Namespace = inf, ElementName = "MT_GRP_PP0100_Con")]
        public Req_PP0100 Req_PP0100 { get; set; }

    }
}
public partial class Req_PP0100
{
    public Req_PP0100()
    {
        Header = new RequestHeader();
        Body = new ReqBody_PP0100();
    }

    [XmlElement(ElementName = "Header", Namespace = "", Order = 0)]
    public RequestHeader Header { get; set; }

    [XmlElement(ElementName = "Body", Namespace = "", Order = 1)]
    public ReqBody_PP0100 Body { get; set; }
}

public partial class ReqBody_PP0100
{
    public ReqBody_PP0100()
    {
        ZPP6210Ts = new ZPP6210T[] { new ZPP6210T() };
    }

    [XmlElement(ElementName = "ZPP6210T")]
    public ZPP6210T[] ZPP6210Ts { get; set; }

}
public class ZPP6210T
{
    public string WERKS { get; set; } = string.Empty;   // 플랜트	
    public string BUDAT { get; set; } = string.Empty;      // 전기(GR)일자	
    public string ZSERNO { get; set; } = string.Empty;     // 일련번호	
    public string MATNR { get; set; } = string.Empty;    // 출고자재코드	
    public string CHARG { get; set; } = string.Empty;     // 출고 Batch No.	
    public string ZPKG_NO { get; set; } = string.Empty;     // 출고 SUB LOT 번호	
    public string AUFNR { get; set; } = string.Empty;     // 생산오더 번호	
    public string ZJOBNO { get; set; } = string.Empty;     // 작업지시번호	
    public string VORNR { get; set; } = string.Empty;     // 촐고공정번호	
    public string BWART { get; set; } = string.Empty;     // 이동유형(261,262)	
    public string RSNUM { get; set; } = string.Empty;     // 출고예약번호	
    public string MENGE { get; set; } = string.Empty;     // 투입출고수량(기본단위)	
    public string MEINS { get; set; } = string.Empty;     // 기본단위	
    public string ERFMG { get; set; } = string.Empty;     // 출고수량(생산단위)	
    public string ERFME { get; set; } = string.Empty;     // 생산단위	
    public string LGORT { get; set; } = string.Empty;     // 자재창고	
    public string MJAHR { get; set; } = string.Empty;     // 자재문서년도(사용안함)	
    public string MBLNR { get; set; } = string.Empty;     // 자재문서번호(사용안함)	
    public string KZEAR { get; set; } = string.Empty;     // ‘ ‘ ? 부분출고, ‘X’ ? 최종출고확정	
    public string DATUV { get; set; } = string.Empty;     // 작업일자(실제출고일자)	
    public string ZBFGI { get; set; } = string.Empty;     // "Backflush 출고요청 - ""X"""	
    public string ZPROC { get; set; } = string.Empty;     // 실적처리상태('N'-미처리,'Y'-처리완료,'E'-에러발생)	
    public string MDATE { get; set; } = string.Empty;     // MES 생성일자	
    public string ZDATS { get; set; } = string.Empty;     // SAP 처리일자	
    public string ZMSG { get; set; } = string.Empty;     // 처리 Message	
    public string GRUND { get; set; } = string.Empty;     // 이동 사유	
}
