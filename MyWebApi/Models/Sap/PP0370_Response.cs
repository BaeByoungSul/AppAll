using Mysqlx.Crud;
using System.Xml.Serialization;
using static Models.Sap.PP0370_Response;

namespace Models.Sap;

/// <summary>
/// Reponse 샘플
/// <SOAP:Envelope xmlns:SOAP="http://schemas.xmlsoap.org/soap/envelope/">
///   <SOAP:Header/>
///   <SOAP:Body xmlns:inf="http://grpeccpp.esp.com/infesp">
///      <ns0:MT_GRP_PP0370_Con_response xmlns:ns0="http://grpeccpp.esp.com/infesp">
///         <Header>
///            <zResultCd>S</zResultCd>
///            <zResultMsg>Success</zResultMsg>
///         </Header>
///         <Body>
///            <STOCK_LIST>
///               <WERKS>5131</WERKS>
///               <MATNR>10352688</MATNR>
///               
/// 하위레벨부터 Class정의
/// 1. STOCK_LIST >> Body
/// 2. Response Header 
/// 3. MT_GRP_PP0370_Con_response ( 1, 2 )
/// 4. SOAP Body 정의
/// 5. Envelop class 정의
/// 
///
/// </summary>

public class PP0370_Response
{
    private const string soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
    private const string inf = "http://grpeccpp.esp.com/infesp";

    //1.  STOCK_LIST(외부파일) >> Body
    public class ResponseBody
    {
        [XmlElement(ElementName = "STOCK_LIST")]
        public List<SapStock> STOCK_LIST { get; set; } = new List<SapStock>();

    }
    // 2. Response Header: 외부파일

    // 3. MT_GRP_PP0370_Con_response

    public class MT_GRP_PP0370_Con_response
    {

        [XmlElement(ElementName = "Header", Namespace = "", Order = 0)]
        public ResponseHeader Header { get; set; } = new ResponseHeader();

        [XmlElement(ElementName = "Body", Namespace = "", Order = 1)]
        public ResponseBody Body { get; set; } = new ResponseBody();
    }

    // 4. Soap Body정의
    public class EnvBody
    {
        [XmlElement(ElementName = "MT_GRP_PP0370_Con_response", Namespace = "http://grpeccpp.esp.com/infesp")]
        public MT_GRP_PP0370_Con_response res { get; set; } = new MT_GRP_PP0370_Con_response();
    }

    // 5. Envelop Class정의
    [XmlRoot(Namespace = soapenv)]
    public class Envelope
    {
        [XmlElement(ElementName = "Header", Namespace = soapenv)]
        public EnvHeader? Header { get; set; } = new EnvHeader();

        [XmlElement(ElementName = "Body", Namespace = soapenv)]
        public EnvBody? Body { get; set; }

        public static XmlSerializerNamespaces Getxmlns()
        {
            var xmlns = new XmlSerializerNamespaces();

            //xmlns.Add("inf", inf);
            xmlns.Add("ns0", inf);
            xmlns.Add("SOAP", soapenv);
            return xmlns;
        }
    }
    public class EnvHeader
    {

    }


};
