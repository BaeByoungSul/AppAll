using System.Xml;

namespace Models.Sap;

[DataContract]
public class SapRequest
{
    [DataMember]
    public string requestXml { get; set; } = string.Empty;

    [DataMember]
    public string requestUrl { get; set; } = string.Empty;

}
