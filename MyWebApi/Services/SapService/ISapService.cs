using Models.Sap.PP0370;

namespace Services.SapService;
public interface ISapService
{
    public Task<HttpResponseMessage> Request_Sap(string reqXml, string url);
    public Task<HttpResponseMessage> Request_PP0370(PP0370_Request req, string url);
    
}
