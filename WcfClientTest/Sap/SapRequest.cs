using System.Runtime.Serialization;
using System.Xml;

namespace Models.Sap;

[DataContract]
public class SapRequest
{
    [DataMember]
    public string requestXml { get; set; }

    [DataMember]
    public string requestUrl { get; set; }

}
