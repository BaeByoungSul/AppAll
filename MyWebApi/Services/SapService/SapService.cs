using Models.Sap;
using System.Net.Http.Headers;
using System.Text;

namespace Services.SapService;

public class SapService : ISapService
{
    private readonly string clientId;
    private readonly string clientSecret;

    private readonly string SoapAction = @"http://sap.com/xi/WebService/soap1.1";
    public SapService(IConfiguration configuration)
    {
        clientId = configuration.GetValue<string>("SapAuth:BasicUser");
        clientSecret = configuration.GetValue<string>("SapAuth:BasicPassword");

    }

    /// <summary>
    /// 1. Client에서 request를 xml serialize해서 string으로 실행할 때 사용
    /// </summary>
    /// <param name="reqXml"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> Request_Sap(string reqXml, string url)
    {
        try
        {
            var authenticationString = $"{clientId}:{clientSecret}";
            byte[] authBytes = Encoding.ASCII.GetBytes(authenticationString);
            
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var reqContent = new StringContent(reqXml, Encoding.UTF8, "text/xml");
            request.Content = reqContent;

            request.Headers.Add("SOAPAction", SoapAction);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

            return await client.SendAsync(request);
           
        }
        catch (Exception)
        {

            throw;
        }
        
        
    }
    /// <summary>
    /// 1. request 를 xml serialize해서 Http SendAsync
    /// </summary>
    /// <param name="req"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> Request_PP0370(PP0370_Request req, string url)
    {
        try
        {
            var reqXml = ReqString_PP0370(req);
            var rtn = await Request_Sap(reqXml, url);
            return rtn;

        }
        catch (Exception)
        {

            throw;
        }


    }
    public async Task<HttpResponseMessage> Request_PP0370_bak(PP0370_Request req, string url)
    {
        try
        {
            var authenticationString = $"{clientId}:{clientSecret}";
            byte[] authBytes = Encoding.ASCII.GetBytes(authenticationString);
            
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var reqXml = ReqString_PP0370(req);

            var reqContent = new StringContent( reqXml, Encoding.UTF8, "text/xml");
            request.Content = reqContent;

            request.Headers.Add("SOAPAction", SoapAction);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

            return await client.SendAsync(request);

        }
        catch (Exception)
        {

            throw;
        }


    }
    private static string ReqString_PP0370(PP0370_Request req)
    {
        Type type = typeof(PP0370.Envelope);

        PP0370.Envelope envelope = new()
        {
            Header = new PP0370.EnvHeader(),
            Body = new PP0370.EnvBody()
            {
                Req_PP0370 = req
            }
        };
        var reqString = DataSerializer.XmlSerialize(type, envelope, envelope.Xmlns);

        Console.WriteLine(reqString);
        return reqString;
    }

}
