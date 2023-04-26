using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sap.PP0060;


[MessageContract(IsWrapped = false)]
public class PP0060_Request
{
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://grpeccpp.esp.com/infesp", Order = 0)]
    public DT_GRP_PP0060_Con? MT_GRP_PP0060_Con;

}
public class DT_GRP_PP0060_Con
{
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public RequestHeader? Header { get; set; }

    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public DT_PP0060_ReqBody? Body { get; set; }
}
public partial class DT_PP0060_ReqBody
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string? WERKS { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
    public string? SPMON { get; set; }
}
