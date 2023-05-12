
using Models.Sap;
using System.Xml;

namespace Services.Sap;

[ServiceContract]
public interface ISapService
{
    [OperationContract]
    Task<XmlElement?> Request_Sap(SapRequest req);
    
    
}
