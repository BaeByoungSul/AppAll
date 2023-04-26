using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SapWcf.PP0060;
using Services.SapService;
using Services.SapWcfService;
using System.Net;
using System.Xml.Serialization;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SapWcfController : ControllerBase
    {
        private readonly string PP0060_QAS = "http://infheaidrdb01.kolon.com:51000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_QAS&receiverParty=&receiverService=&interface=SI_GRP_PP0060_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp\r\n";
        private readonly SapWcfService _sapService;

        public SapWcfController(SapWcfService sapService)
        {
            _sapService = sapService;
        }
        [HttpPost("PP0060_Interface")]
        public  IActionResult PP0060(PP0060_Request req)
        {
            try
            {
                string url = PP0060_QAS;
      

                var response = _sapService.Request_PP0060(req, url);
                return Ok(response);


            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, title: ex.HResult.ToString());
                //return BadRequest(ex.Message);
            }
        }

    }
}
