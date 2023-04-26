using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sap.PP0370;

[System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
public partial class PP0370_Request
{

    [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://grpeccpp.esp.com/infesp", Order = 0)]
    public DT_GRP_PP0370_Con? MT_GRP_PP0370_Con;

    public PP0370_Request()
    {
    }

    public PP0370_Request(DT_GRP_PP0370_Con MT_GRP_PP0370_Con)
    {
        this.MT_GRP_PP0370_Con = MT_GRP_PP0370_Con;
    }
}

public partial class DT_GRP_PP0370_Con
{

    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public RequestHeader Header { get; set; }

    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public DT_ReqBody Body { get; set; }

}


public partial class DT_ReqBody
{


    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("T_WERKS", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public CWERKS[] T_WERKS { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("T_MATNR", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
    public CMATNR[] T_MATNR { get; set; }

    /// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute("T_MTART", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
    //public DT_GRP_PP0370_ConBodyT_MTART[] T_MTART { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("T_DISPO", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
    public CDISPO[] T_DISPO
    {
        get; set;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("T_CHARG", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
    public CCHARG[] T_CHARG
    {
        get; set;
    }

    /// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute("T_GRADE", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
    //public DT_GRP_PP0370_ConBodyT_GRADE[] T_GRADE
    //{
    //    get; set;
    //}

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("T_LGORT", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
    public CLGORT[] T_LGORT
    {
        get; set;
    }

    /// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute("T_LIFNR", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
    //public DT_GRP_PP0370_ConBodyT_LIFNR[] T_LIFNR
    //{
    //    get; set;
    //}

    /// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute("T_MATNR_A", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
    //public DT_GRP_PP0370_ConBodyT_MATNR_A[] T_MATNR_A
    //{
    //    get; set;
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute("T_BISMT", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 9)]
    //public DT_GRP_PP0370_ConBodyT_BISMT[] T_BISMT
    //{
    //    get; set;
    //}
}


public class CWERKS {
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string WERKS { get; set; } 
}
public class CMATNR {
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string? MATNR { get; set; }
}
public class CDISPO { [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string? DISPO { get; set; } }
public class CCHARG { [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string? CHARG { get; set; } }
public class CLGORT { [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string? LGORT { get; set; } }