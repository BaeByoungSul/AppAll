
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Models.Sap;
using MyWebApi.Models.Sap.PP0370;
using Sap;
using Services.SapService;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace MyWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class SapController : ControllerBase
{
    private readonly ISapService _sapService;
    private readonly string PP0370_QAS = "http://infheaidrdb01.kolon.com:51000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_QAS&receiverParty=&receiverService=&interface=SI_GRP_PP0370_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp";
    private readonly string PP0370_PRD = "http://infheaidrdb01.kolon.com:51000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_QAS&receiverParty=&receiverService=&interface=SI_GRP_PP0370_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp";

    //http://infheaidrdb01.kolon.com:51000/dir/wsdl?p=ic/8b24bde7a5a136b7b5b2e341913c80d2

    public SapController(ISapService sapService)
    {
        _sapService = sapService;
    }
    /// <summary>
    /// 1. Api Client에서 요청 xml string을 만들어서 sap i/f
    /// </summary>
    /// <param name="reqXml"></param>
    /// <param name="enumUrl"></param>
    /// <returns></returns>
    [HttpGet("SapRequest")]
    public async Task<IActionResult> SapRequest(string reqXml, string sUrl)
    {
        try
        {

            var response = await _sapService.Request_Sap(reqXml, sUrl);
            if (response.StatusCode != HttpStatusCode.OK)
                return Problem(statusCode: (int)response.StatusCode, title: response.ReasonPhrase);

            this.HttpContext.Response.RegisterForDispose(response);
            return new HttpResponseMessageResult(response);

        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: ex.HResult.ToString());
            //return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// SAP,NetWeaver Http Response 를 Return( xml response )
    /// </summary>
    /// <param name="req"></param>
    /// <param name="enumUrl"></param>
    /// <returns></returns>
    [HttpPost("PP0370_XML")]
    public async Task<IActionResult>PP0370_XML(PP0370_Request req, EnumUrl enumUrl)
    {
        string url = string.Empty;
        if (enumUrl  == EnumUrl.PP0370_QAS)
            url = PP0370_QAS;
        else if( enumUrl == EnumUrl.PP0370_PROD) 
            url = PP0370_PRD;
        else
            return BadRequest("Invale Url");
        var response = await _sapService.Request_PP0370(req, url);
        if (response.StatusCode != HttpStatusCode.OK)
            return Problem(statusCode: (int)response.StatusCode, title: response.ReasonPhrase);

        this.HttpContext.Response.RegisterForDispose(response);
        return new HttpResponseMessageResult(response);
    }

    

    /// <summary>
    /// 1. Http Response Message를 읽고 Namespace를 제거 후 Deserialize
    /// 2. Class정의 시 Namespace제거
    /// </summary>
    /// <param name="req"></param>
    /// <param name="enumUrl"></param>
    /// <returns></returns>
    [HttpPost("PP0370_JSON")]
    public async Task<IActionResult> PP0370_JSON(PP0370_Request req, EnumUrl enumUrl)
    {
        try
        {
            string url = string.Empty;
            if (enumUrl == EnumUrl.PP0370_QAS)
                url = PP0370_QAS;
            else if (enumUrl == EnumUrl.PP0370_PROD)
                url = PP0370_PRD;
            else
                return BadRequest("Invale Url");
            var response = await _sapService.Request_PP0370(req, url);

            if (response.StatusCode != HttpStatusCode.OK)
                return Problem(statusCode: (int)response.StatusCode, title: response.ReasonPhrase);

            var resMsg = await response.Content.ReadAsStringAsync();
            var sr = new StringReader(resMsg);
            var xmlSerializer = new XmlSerializer(typeof(PP0370_Response2.Envelope));
            PP0370_Response2.Envelope? envelope = xmlSerializer.Deserialize(new IgnoreNamespaceXmlTextReader(sr)) as PP0370_Response2.Envelope;
            return Ok(envelope?.Body?.res);

            //if(response.StatusCode != HttpStatusCode.OK) 

            //var resMsg = await response.Content.ReadAsStreamAsync();

            //XmlSerializer ser = new(typeof(PP0370_Response.Envelope));
            //PP0370_Response.Envelope envelope = (PP0370_Response.Envelope)ser.Deserialize(resMsg);
            //return Ok(envelope.Body.res);


        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: ex.HResult.ToString());
            //return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 1. Http Response Message를 읽고 Namespace를 고려하여 Deserialize
    /// 2. Class정의 후 Serialize Test ( SoapUI 참조  )
    /// </summary>
    /// <param name="req"></param>
    /// <param name="enumUrl"></param>
    /// <returns></returns>
    [HttpPost("PP0370_Test2")]
    public async Task<IActionResult> PP0370_Test2(PP0370_Request req, EnumUrl enumUrl)
    {
        try
        {
            string url = string.Empty;
            if (enumUrl == EnumUrl.PP0370_QAS)
                url = PP0370_QAS;
            else if (enumUrl == EnumUrl.PP0370_PROD)
                url = PP0370_PRD;
            else
                return BadRequest("Invalid Url");

            var response = await _sapService.Request_PP0370(req, url);

            if (response.StatusCode != HttpStatusCode.OK)
                return Problem(statusCode: (int)response.StatusCode, title: response.ReasonPhrase);

  
            var resMsg = await response.Content.ReadAsStreamAsync();

            XmlSerializer ser = new(typeof(PP0370_Response.Envelope));
            PP0370_Response.Envelope? envelope = ser.Deserialize(resMsg) as PP0370_Response.Envelope;
            return Ok(envelope?.Body?.res);


        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: ex.HResult.ToString());
            //return BadRequest(ex.Message);
        }
    }


}

/// <summary>
/// https://github.com/dotnet/aspnetcore/issues/26216
/// https://stackoverflow.com/questions/54136488/correct-way-to-return-httpresponsemessage-as-iactionresult-in-net-core-2-2
/// </summary>
public class HttpResponseMessageResult : IActionResult
{
    private readonly HttpResponseMessage _responseMessage;

    public HttpResponseMessageResult(HttpResponseMessage responseMessage)
    {
        _responseMessage = responseMessage; // could add throw if null
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;


        if (_responseMessage == null)
        {
            var message = "Response message cannot be null";

            throw new InvalidOperationException(message);
        }

        using (_responseMessage)
        {
            response.StatusCode = (int)_responseMessage.StatusCode;

            var responseFeature = context.HttpContext.Features.Get<IHttpResponseFeature>();
            if (responseFeature != null)
            {
                responseFeature.ReasonPhrase = _responseMessage.ReasonPhrase;
            }

            var responseHeaders = _responseMessage.Headers;

            // Ignore the Transfer-Encoding header if it is just "chunked".
            // We let the host decide about whether the response should be chunked or not.
            if (responseHeaders.TransferEncodingChunked == true &&
                responseHeaders.TransferEncoding.Count == 1)
            {
                responseHeaders.TransferEncoding.Clear();
            }

            foreach (var header in responseHeaders)
            {
                response.Headers.Append(header.Key, header.Value.ToArray());
            }

            if (_responseMessage.Content != null)
            {
                var contentHeaders = _responseMessage.Content.Headers;

                // Copy the response content headers only after ensuring they are complete.
                // We ask for Content-Length first because HttpContent lazily computes this
                // and only afterwards writes the value into the content headers.
                var unused = contentHeaders.ContentLength;

                foreach (var header in contentHeaders)
                {
                    response.Headers.Append(header.Key, header.Value.ToArray());
                }

                await _responseMessage.Content.CopyToAsync(response.Body);
            }
        }
    }
}
