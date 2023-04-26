using System.Xml.Serialization;

namespace MyWebApi.Models.Sap.PP0370;

public class SapStock
{
    [XmlElement(ElementName = "WERKS")]
    public string? WERKS { get; set; }

    [XmlElement(ElementName = "MATNR")]
    public string? MATNR { get; set; }

    [XmlElement(ElementName = "MAKTX")]
    public string? MAKTX { get; set; }

    [XmlElement(ElementName = "LGORT")]
    public string? LGORT { get; set; }

    [XmlElement(ElementName = "LIFNR")]
    public string? LIFNR { get; set; }

    [XmlElement(ElementName = "CHARG")]
    public string? CHARG { get; set; }

    [XmlElement(ElementName = "ZZCHARG")]
    public string? ZZCHARG { get; set; }

    [XmlElement(ElementName = "ZZGRADE")]
    public string? ZZGRADE { get; set; }


    [XmlElement(ElementName = "MEINS")]
    public string? MEINS { get; set; }

    [XmlElement(ElementName = "KALAB")]
    public string? KALAB { get; set; }

    [XmlElement(ElementName = "INSME")]
    public string? INSME { get; set; }

    [XmlElement(ElementName = "SPEME")]
    public string? SPEME { get; set; }

    [XmlElement(ElementName = "UMLME")]
    public string? UMLME { get; set; }

    [XmlElement(ElementName = "AUFNR")]
    public string? AUFNR { get; set; }

    [XmlElement(ElementName = "BUDAT")]
    public string? BUDAT { get; set; }

    [XmlElement(ElementName = "ATNAM01")]
    public string? ATNAM01 { get; set; }

    [XmlElement(ElementName = "ATWRT01")]
    public string? ATWRT01 { get; set; }


};
