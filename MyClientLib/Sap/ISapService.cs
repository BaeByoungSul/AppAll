
using Models.Sap;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;

namespace Services.Sap
{

    [ServiceContract]
    public interface ISapService
    {
        [OperationContract]
        Task<XmlElement> Request_Sap(SapRequest req);


    }

}
