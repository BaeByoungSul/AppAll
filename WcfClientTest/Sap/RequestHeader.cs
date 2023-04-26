using System.Xml.Serialization;

namespace Sap;
public partial class RequestHeader
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("zInterfaceId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string? ZInterfaceId { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("zConSysId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
    public string? ZConSysId { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("zProSysId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
    public string? ZProSysId { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("zUserId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
    public string? ZUserId { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("zPiUser", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
    public string? ZPiUser { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("zTimeId", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
    public string? ZTimeId { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("zLang", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
    public string? ZLang { get; set; }
}