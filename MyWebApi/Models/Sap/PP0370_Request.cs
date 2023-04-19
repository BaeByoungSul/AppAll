using System.Xml.Serialization;

namespace Models.Sap;

public class PP0370_Request
{
    public PP0370_Request()
    {
        Header = new ReqHeader();
        Body = new PP0370_ReqBody();
    }

    [XmlElement(ElementName = "Header", Namespace = "", Order = 0)]
    public ReqHeader Header { get; set; }

    [XmlElement(ElementName = "Body", Namespace = "", Order = 1)]
    public PP0370_ReqBody Body { get; set; }
}

public class PP0370_ReqBody
{

    [XmlElement(ElementName = "T_WERKS")]
    public List<CWERKS> TWERKS { get; set; } = new List<CWERKS>();

    [XmlElement(ElementName = "T_MATNR")]
    public List<CMATNR> TMATNR { get; set; } = new List<CMATNR>();

    [XmlElement(ElementName = "T_DISPO")]
    public List<CDISPO> TDISPO { get; set; } = new List<CDISPO>();

    [XmlElement(ElementName = "T_CHARG")]
    public List<CCHARG> TCHARG { get; set; } = new List<CCHARG>();

    [XmlElement(ElementName = "T_LGORT")]
    public List<CLGORT> TLGORT { get; set; } = new List<CLGORT>();

    public void AddWerks(string werks) => TWERKS.Add(new CWERKS { WERKS = werks });
    public void AddMatnr(string matnr) => TMATNR.Add(new CMATNR { MATNR = matnr });
    public void AddDispo(string dispo) => TDISPO.Add(new CDISPO { DISPO = dispo });
    public void AddCharg(string charg) => TCHARG.Add(new CCHARG { CHARG = charg });
    public void AddLgort(string lgort)
    {
        TLGORT.Add(new CLGORT { LGORT = lgort });
    }


    public class CWERKS { public string WERKS { get; set; } = string.Empty; }
    public class CMATNR { public string? MATNR { get; set; } }
    public class CDISPO { public string? DISPO { get; set; } }
    public class CCHARG { public string? CHARG { get; set; } }
    public class CLGORT { public string? LGORT { get; set; } }
}
