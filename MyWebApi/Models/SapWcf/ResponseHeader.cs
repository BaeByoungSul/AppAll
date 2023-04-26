using System.Xml.Serialization;

namespace SapWcf;


//public class ResponseHeader
//{

//    [XmlElement(ElementName = "zResultCd")]
//    public string? ZResultCd { get; set; }

//    [XmlElement(ElementName = "zResultMsg")]
//    public string? ZResultMsg { get; set; }

//}
public class ResponseHeader
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElement("zResultCd", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string? ZResultCd { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElement("zResultMsg", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
    public string? ZResultMsg { get; set; }
}

