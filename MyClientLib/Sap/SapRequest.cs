using System.Runtime.Serialization;
using System.Xml;

namespace Models.Sap
{
    [DataContract]
    public class SapRequest
    {
        [DataMember]
        public string RequestXml { get; set; } = string.Empty;

        [DataMember]
        public string RequestUrl { get; set; } = string.Empty;

    }

}

