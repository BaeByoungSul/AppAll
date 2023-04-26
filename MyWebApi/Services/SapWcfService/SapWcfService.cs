using SapWcf;
using SapWcf.PP0060;
using System.ServiceModel;

namespace Services.SapWcfService;

public class SapWcfService
{
    private static readonly string PP0060_QAS = "http://infheaidrdb01.kolon.com:51000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_QAS&receiverParty=&receiverService=&interface=SI_GRP_PP0060_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp\r\n";


    public PP0060_Response Request_PP0060(PP0060_Request req, string url)
    {
        // This code is written by an application developer.
        // Create a channel factory.
        //BasicHttpBinding myBinding = new BasicHttpBinding();
        PP0060_Request request = Get_Req_PP0060();

        //Console.WriteLine(XmlSerialize(typeof(SI_GRP_PP0370_SORequest),
        //                request, null));

        BasicHttpBinding myBinding = GetSapHttpBinding();

        EndpointAddress myEndpoint = new EndpointAddress(PP0060_QAS);

        ChannelFactory<ISapPP0060> cf = new ChannelFactory<ISapPP0060>(myBinding, myEndpoint);
        cf.Credentials.UserName.UserName = "IF_KIICHA";
        cf.Credentials.UserName.Password = "Interface!12";
        // Create a channel.
        ISapPP0060 _cli = cf.CreateChannel();

        var rtn = _cli.SI_GRP_PP0060_SO(request);
        // Console.WriteLine(rtn);

        ((IClientChannel)_cli).Close();

        cf.Close();
        return rtn;

    }

    private static PP0060_Request Get_Req_PP0060()
    {

        RequestHeader reqHeader = new RequestHeader()
        {
            ZInterfaceId = "GRP_PP0370",
            ZConSysId = "KII_CHA",
            ZProSysId = "GRP_ECC_PP",
            ZPiUser = "IF_KIICHA",
            ZTimeId = DateTime.Now.ToString("yyyyMMddHHmmss"),
            ZUserId = "BBS"
        };


        // mt.Header = reqHeader;

        //private DT_ReqHeader headerField;
        // private DT_GRP_PP0370_ConBody bodyField;
        PP0060_Request req = new PP0060_Request()
        {
            MT_GRP_PP0060_Con = new DT_GRP_PP0060_Con()
            {
                Header = reqHeader,
                Body = new DT_PP0060_ReqBody()
                {
                    WERKS = "5131",
                    SPMON = "202301"
                }
            }
        };

        return req;

    }
    private static BasicHttpBinding GetSapHttpBinding()
    {
        BasicHttpBinding binding = new BasicHttpBinding();

        binding.TransferMode = TransferMode.Buffered;
        //binding.MessageEncoding = WSMessageEncoding.Mtom;
        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        binding.MaxBufferSize = 2147483647;
        binding.MaxReceivedMessageSize = 2147483647;

        binding.OpenTimeout = TimeSpan.FromMinutes(5);
        binding.CloseTimeout = TimeSpan.FromMinutes(5);
        binding.ReceiveTimeout = TimeSpan.FromMinutes(15);
        binding.SendTimeout = TimeSpan.FromMinutes(15);

        binding.ReaderQuotas.MaxStringContentLength = 2147483647;

        return binding;
    }

}
