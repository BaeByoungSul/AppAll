using Mysqlx.Crud;
using System.Xml.Serialization;

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
/// . Namespace 부분 제거하고 생성
/// . 하위레벨부터 Class정의
///    1. STOCK_LIST >> Body
///    2. Response Header 
///    3. MT_GRP_PP0370_Con_response ( 1, 2 )
///    4. SOAP Body 정의
///    5. Envelop class 정의
///
/// </summary>

public class PP0370_Response2
{


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

        [XmlElement(ElementName = "Header")]
        public ResponseHeader Header { get; set; } = new ResponseHeader();

        [XmlElement(ElementName = "Body")]
        public ResponseBody Body { get; set; } = new ResponseBody();
    }

    // 4. Soap Body정의
    public class EnvBody
    {
        [XmlElement(ElementName = "MT_GRP_PP0370_Con_response")]
        public MT_GRP_PP0370_Con_response res { get; set; } = new MT_GRP_PP0370_Con_response();
    }

    // 5. Envelop Class정의
    [XmlRoot]
    public class Envelope
    {
        [XmlElement(ElementName = "Header")]
        public EnvHeader? Header { get; set; } = new EnvHeader();

        [XmlElement(ElementName = "Body")]
        public EnvBody? Body { get; set; }

    }
    public class EnvHeader
    {

    }


 

};

