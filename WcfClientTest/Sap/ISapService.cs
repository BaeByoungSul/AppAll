

using Models.Sap;
using System.ServiceModel;
using System.Xml;

namespace Sap;

[ServiceContract]
public interface ISapService
{
    [OperationContract]
    public XmlElement Request_Sap(SapRequest request);
    
    
}
