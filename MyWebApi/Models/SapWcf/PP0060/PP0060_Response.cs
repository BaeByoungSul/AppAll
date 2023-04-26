
using System.ServiceModel;
using System.Xml.Serialization;
using Models.Sap;



namespace SapWcf.PP0060;

[MessageContract(IsWrapped = false)]
public partial class PP0060_Response
{

    //[MessageBodyMember(Namespace = "http://grpeccpp.esp.com/infesp", Order = 0)]
    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://grpeccpp.esp.com/infesp", Order = 0)]
    //public ReponseAll_PP0060? 0_Con_response { get;set; }
    public ReponseAll_PP0060? MT_GRP_PP0060_Con_response { get; set; }
    //public PP0370_Response()
    //{
    //}
    //public PP0370_Response(ReponseAll MT_GRP_PP0370_Con_response)
    //{
    //    this.MT_GRP_PP0370_Con_response = MT_GRP_PP0370_Con_response;
    //}
}

/// <remarks/>
public class ReponseAll_PP0060
{

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public ResponseHeader? Header { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
    public ResponseBody_PP0060? Body { get; set; }

}
public class ResponseBody_PP0060
{
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string? WERKS { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
    public string? SPMON { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
    public string? ZZXRUEM { get; set; }

}