using System.Xml.Serialization;

namespace Models.Sap;


public class ResponseHeader
{

    [XmlElement(ElementName = "zResultCd")]
    public string? ZResultCd { get; set; }

    [XmlElement(ElementName = "zResultMsg")]
    public string? ZResultMsg { get; set; }

}
