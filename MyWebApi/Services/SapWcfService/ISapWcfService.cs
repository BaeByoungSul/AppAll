using Models.Sap;
using SapWcf.PP0060;

namespace Services.SapWcfService;
public interface ISapWcfService
{
    //public Task<HttpResponseMessage> Request_Sap(string reqXml, string url);
    //public Task<HttpResponseMessage> Request_PP0370(PP0370_Request req, string url);
    public PP0060_Response Request_PP0060( PP0060_Request req, string url);

}
