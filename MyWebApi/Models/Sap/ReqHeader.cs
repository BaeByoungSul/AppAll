using System.Xml.Serialization;

namespace Models.Sap;

public class ReqHeader
{
    [XmlElement(ElementName = "zInterfaceId")]
    public string ZInterfaceId { get; set; }= String.Empty;

    [XmlElement(ElementName = "zConSysId")]
    public string ZConSysId { get; set; } = String.Empty;

    [XmlElement(ElementName = "zProSysId")]
    public string ZProSysId { get; set; } = String.Empty;

    [XmlElement(ElementName = "zUserId")]
    public string ZUserId { get; set; } = String.Empty;

    [XmlElement(ElementName = "zPiUser")]
    public string ZPiUser { get; set; } = String.Empty;

    [XmlElement(ElementName = "zTimeId")]
    public string ZTimeId { get; set; } = String.Empty;

    [XmlElement(ElementName = "zLang")]
    public string ZLang { get; set; } = String.Empty;

}