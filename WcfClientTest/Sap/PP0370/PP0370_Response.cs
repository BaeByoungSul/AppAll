
using System.ServiceModel;
using System.Xml.Serialization;
using Sap;

namespace Sap.PP0370;


[MessageContract(IsWrapped = false)]
public partial class PP0370_Response
{

    [MessageBodyMember(Namespace = "http://grpeccpp.esp.com/infesp", Order = 0)]
    public ReponseAll? MT_GRP_PP0370_Con_response;
    //public PP0370_Response()
    //{
    //}
    //public PP0370_Response(ReponseAll MT_GRP_PP0370_Con_response)
    //{
    //    this.MT_GRP_PP0370_Con_response = MT_GRP_PP0370_Con_response;
    //}
}

/// <remarks/>
public class ReponseAll
{

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public ResponseHeader? Header { get; set; }

    /// <remarks/>
    [XmlArray(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
    [XmlArrayItem("STOCK_LIST", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
    public ResponseStock[]? Body { get; set; }

}
public class ResponseStock
{
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
    public string? WERKS { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
    public string? MATNR { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
    public string? MAKTX { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
    public string? LGORT { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
    public string LIFNR { get; set; }
    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
    public string? CHARG { get; set; }
    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
    public string? ZZCHARG { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
    public string ZZGRADE { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
    public string? MEINS { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 9)]
    public string KALAB { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 10)]
    public string? INSME { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
    public string SPEME { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 12)]
    public string? UMLME { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 13)]
    public string AUFNR { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 14)]
    public string? EBELN { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 15)]
    public string EBELP { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 16)]
    public string? BUDAT { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 17)]
    public string? ATNAM01 { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 18)]
    public string? ATWRT01 { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 19)]
    public string? ATNAM02 { get; set; }

    /// <remarks/>
    [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 20)]
    public string? ATWRT02 { get; set; }

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 21)]
    //public string ATNAM03
    //{
    //    get
    //    {
    //        return this.aTNAM03Field;
    //    }
    //    set
    //    {
    //        this.aTNAM03Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 22)]
    //public string ATWRT03
    //{
    //    get
    //    {
    //        return this.aTWRT03Field;
    //    }
    //    set
    //    {
    //        this.aTWRT03Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 23)]
    //public string ATNAM04
    //{
    //    get
    //    {
    //        return this.aTNAM04Field;
    //    }
    //    set
    //    {
    //        this.aTNAM04Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 24)]
    //public string ATWRT04
    //{
    //    get
    //    {
    //        return this.aTWRT04Field;
    //    }
    //    set
    //    {
    //        this.aTWRT04Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 25)]
    //public string ATNAM05
    //{
    //    get
    //    {
    //        return this.aTNAM05Field;
    //    }
    //    set
    //    {
    //        this.aTNAM05Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 26)]
    //public string ATWRT05
    //{
    //    get
    //    {
    //        return this.aTWRT05Field;
    //    }
    //    set
    //    {
    //        this.aTWRT05Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 27)]
    //public string ATNAM06
    //{
    //    get
    //    {
    //        return this.aTNAM06Field;
    //    }
    //    set
    //    {
    //        this.aTNAM06Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 28)]
    //public string ATWRT06
    //{
    //    get
    //    {
    //        return this.aTWRT06Field;
    //    }
    //    set
    //    {
    //        this.aTWRT06Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 29)]
    //public string ATNAM07
    //{
    //    get
    //    {
    //        return this.aTNAM07Field;
    //    }
    //    set
    //    {
    //        this.aTNAM07Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 30)]
    //public string ATWRT07
    //{
    //    get
    //    {
    //        return this.aTWRT07Field;
    //    }
    //    set
    //    {
    //        this.aTWRT07Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 31)]
    //public string ATNAM08
    //{
    //    get
    //    {
    //        return this.aTNAM08Field;
    //    }
    //    set
    //    {
    //        this.aTNAM08Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 32)]
    //public string ATWRT08
    //{
    //    get
    //    {
    //        return this.aTWRT08Field;
    //    }
    //    set
    //    {
    //        this.aTWRT08Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 33)]
    //public string ATNAM09
    //{
    //    get
    //    {
    //        return this.aTNAM09Field;
    //    }
    //    set
    //    {
    //        this.aTNAM09Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 34)]
    //public string ATWRT09
    //{
    //    get
    //    {
    //        return this.aTWRT09Field;
    //    }
    //    set
    //    {
    //        this.aTWRT09Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 35)]
    //public string ATNAM10
    //{
    //    get
    //    {
    //        return this.aTNAM10Field;
    //    }
    //    set
    //    {
    //        this.aTNAM10Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 36)]
    //public string ATWRT10
    //{
    //    get
    //    {
    //        return this.aTWRT10Field;
    //    }
    //    set
    //    {
    //        this.aTWRT10Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 37)]
    //public string ATNAM11
    //{
    //    get
    //    {
    //        return this.aTNAM11Field;
    //    }
    //    set
    //    {
    //        this.aTNAM11Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 38)]
    //public string ATWRT11
    //{
    //    get
    //    {
    //        return this.aTWRT11Field;
    //    }
    //    set
    //    {
    //        this.aTWRT11Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 39)]
    //public string ATNAM12
    //{
    //    get
    //    {
    //        return this.aTNAM12Field;
    //    }
    //    set
    //    {
    //        this.aTNAM12Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 40)]
    //public string ATWRT12
    //{
    //    get
    //    {
    //        return this.aTWRT12Field;
    //    }
    //    set
    //    {
    //        this.aTWRT12Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 41)]
    //public string ATNAM13
    //{
    //    get
    //    {
    //        return this.aTNAM13Field;
    //    }
    //    set
    //    {
    //        this.aTNAM13Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 42)]
    //public string ATWRT13
    //{
    //    get
    //    {
    //        return this.aTWRT13Field;
    //    }
    //    set
    //    {
    //        this.aTWRT13Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 43)]
    //public string ATNAM14
    //{
    //    get
    //    {
    //        return this.aTNAM14Field;
    //    }
    //    set
    //    {
    //        this.aTNAM14Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 44)]
    //public string ATWRT14
    //{
    //    get
    //    {
    //        return this.aTWRT14Field;
    //    }
    //    set
    //    {
    //        this.aTWRT14Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 45)]
    //public string ATNAM15
    //{
    //    get
    //    {
    //        return this.aTNAM15Field;
    //    }
    //    set
    //    {
    //        this.aTNAM15Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 46)]
    //public string ATWRT15
    //{
    //    get
    //    {
    //        return this.aTWRT15Field;
    //    }
    //    set
    //    {
    //        this.aTWRT15Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 47)]
    //public string ATNAM16
    //{
    //    get
    //    {
    //        return this.aTNAM16Field;
    //    }
    //    set
    //    {
    //        this.aTNAM16Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 48)]
    //public string ATWRT16
    //{
    //    get
    //    {
    //        return this.aTWRT16Field;
    //    }
    //    set
    //    {
    //        this.aTWRT16Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 49)]
    //public string ATNAM17
    //{
    //    get
    //    {
    //        return this.aTNAM17Field;
    //    }
    //    set
    //    {
    //        this.aTNAM17Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 50)]
    //public string ATWRT17
    //{
    //    get
    //    {
    //        return this.aTWRT17Field;
    //    }
    //    set
    //    {
    //        this.aTWRT17Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 51)]
    //public string ATNAM18
    //{
    //    get
    //    {
    //        return this.aTNAM18Field;
    //    }
    //    set
    //    {
    //        this.aTNAM18Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 52)]
    //public string ATWRT18
    //{
    //    get
    //    {
    //        return this.aTWRT18Field;
    //    }
    //    set
    //    {
    //        this.aTWRT18Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 53)]
    //public string ATNAM19
    //{
    //    get
    //    {
    //        return this.aTNAM19Field;
    //    }
    //    set
    //    {
    //        this.aTNAM19Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 54)]
    //public string ATWRT19
    //{
    //    get
    //    {
    //        return this.aTWRT19Field;
    //    }
    //    set
    //    {
    //        this.aTWRT19Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 55)]
    //public string ATNAM20
    //{
    //    get
    //    {
    //        return this.aTNAM20Field;
    //    }
    //    set
    //    {
    //        this.aTNAM20Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 56)]
    //public string ATWRT20
    //{
    //    get
    //    {
    //        return this.aTWRT20Field;
    //    }
    //    set
    //    {
    //        this.aTWRT20Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 57)]
    //public string ATNAM21
    //{
    //    get
    //    {
    //        return this.aTNAM21Field;
    //    }
    //    set
    //    {
    //        this.aTNAM21Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 58)]
    //public string ATWRT21
    //{
    //    get
    //    {
    //        return this.aTWRT21Field;
    //    }
    //    set
    //    {
    //        this.aTWRT21Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 59)]
    //public string ATNAM22
    //{
    //    get
    //    {
    //        return this.aTNAM22Field;
    //    }
    //    set
    //    {
    //        this.aTNAM22Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 60)]
    //public string ATWRT22
    //{
    //    get
    //    {
    //        return this.aTWRT22Field;
    //    }
    //    set
    //    {
    //        this.aTWRT22Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 61)]
    //public string ATNAM23
    //{
    //    get
    //    {
    //        return this.aTNAM23Field;
    //    }
    //    set
    //    {
    //        this.aTNAM23Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 62)]
    //public string ATWRT23
    //{
    //    get
    //    {
    //        return this.aTWRT23Field;
    //    }
    //    set
    //    {
    //        this.aTWRT23Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 63)]
    //public string ATNAM24
    //{
    //    get
    //    {
    //        return this.aTNAM24Field;
    //    }
    //    set
    //    {
    //        this.aTNAM24Field = value;
    //    }
    //}

    ///// <remarks/>
    //[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 64)]
    //public string ATWRT24
    //{
    //    get
    //    {
    //        return this.aTWRT24Field;
    //    }
    //    set
    //    {
    //        this.aTWRT24Field = value;
    //    }
    //}
}