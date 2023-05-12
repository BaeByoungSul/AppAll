using Models.Sap;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;

namespace Services.Sap;

public class SapService : ISapService
{
    private readonly string SoapAction = @"http://sap.com/xi/WebService/soap1.1";
    private readonly string clientId;
    private readonly string clientSecret;

    public SapService()
    {
        var _configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false, reloadOnChange: true)
            .Build();

        clientId = _configuration.GetValue<string>("SapAuth:BasicUser");
        clientSecret = _configuration.GetValue<string>("SapAuth:BasicPassword");

        //Console.WriteLine(clientId);
        //Console.WriteLine(clientSecret);
    }

    /// <summary>
    ///   /// Sap Return부분에서 SOAP Body부분 Return한다.
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="FaultException"></exception>
    public async Task<XmlElement?> Request_Sap(SapRequest req)
    {

        //Console.WriteLine(req.requestXml);
        //Console.WriteLine(req.requestUrl);

        try
        {
            var authenticationString = $"{clientId}:{clientSecret}";
            byte[] authBytes = Encoding.ASCII.GetBytes(authenticationString);

            var client = new HttpClient();

            var reqMsg = new HttpRequestMessage(HttpMethod.Post, req.RequestUrl);

            var reqContent = new StringContent(req.RequestXml, Encoding.UTF8, "text/xml");
            reqMsg.Content = reqContent;

            reqMsg.Headers.Add("SOAPAction", SoapAction);
            reqMsg.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

            var response =  await client.SendAsync(reqMsg);

            if (response.StatusCode != HttpStatusCode.OK) return (null);

            var xmlStream = await response.Content.ReadAsStreamAsync();

            return GetSapXmlElement(xmlStream);

        }
        catch (Exception ex)
        {
            throw new FaultException(ex.ToString());
            
        }
    }

    /// <summary>
    /// Sap Return부분에서 SOAP Body부분을 가져온다.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    private static XmlElement? GetSapXmlElement(Stream stream)
    {

        try
        {
            XmlDocument doc = new();
            doc.Load(stream);

            XmlNamespaceManager nsmgr;
            nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("SOAP", "http://schemas.xmlsoap.org/soap/envelope/");

            XmlNode? myNode = doc.DocumentElement?.SelectSingleNode("/SOAP:Envelope/SOAP:Body", nsmgr);

            if (myNode?.FirstChild?.NodeType == XmlNodeType.Element)
                return myNode?.FirstChild as XmlElement;
            else
                return null ;
        }
        catch (Exception )
        {
            throw;
        }
    }
    //private string PP0370_REQ()
    //{

    //    Dictionary<string, string> headerDic = GetHeaderDic();


    //    StringBuilder sb = new StringBuilder();
    //    StringWriter strw = new StringWriter(sb);

    //    using (XmlTextWriter w = new XmlTextWriter(strw))
    //    {
    //        w.Formatting = Formatting.Indented;

    //        w.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
    //        w.WriteAttributeString("xmlns", "inf", null, "http://grpeccpp.esp.com/infesp");

    //        w.WriteStartElement("soapenv", "Header", null);
    //        w.WriteEndElement(); // End Of soapenv:Header

    //        w.WriteStartElement("soapenv", "Body", null);
    //        w.WriteStartElement("inf", "MT_GRP_PP0370_Con", null);

    //        // 공통부분
    //        w.WriteStartElement("Header");
    //        {
    //            foreach (KeyValuePair<string, string> di in headerDic)
    //            {
    //                w.WriteElementString(di.Key, di.Value);
    //            }

    //        }
    //        w.WriteEndElement(); // End Of Header
    //                             // 공통부분 끝


    //        w.WriteStartElement("Body");
    //        {
                
    //            w.WriteStartElement("T_WERKS");
    //            w.WriteElementString("WERKS", "5131");
    //            w.WriteEndElement();

    //            w.WriteStartElement("T_MATNR");
    //            w.WriteElementString("MATNR", "10352688");
    //            w.WriteEndElement();

    //            w.WriteStartElement("T_LGORT");
    //            w.WriteElementString("LGORT", "5701");
    //            w.WriteEndElement();

    //        }

    //        w.WriteEndElement(); // End Of Body


    //        w.WriteEndElement(); // End Of inf:MT_GRP_PP0370_Con
    //        w.WriteEndElement(); // End Of soapenv:Body
    //        w.WriteEndElement(); // End Of First Start
    //        w.Close();

    //    }


    //    return sb.ToString();
    //    //Console.WriteLine(strw.ToString());

    //    //XmlDocument xmlDoc = new XmlDocument();
    //    //xmlDoc.LoadXml(sb.ToString());
    //    //return xmlDoc;
    //}

    //private Dictionary<string, string> GetHeaderDic()
    //{
    //    Dictionary<string, string> headerDic = new Dictionary<string, string>();
    //    headerDic.Add("zInterfaceId", "GRP_PP0100");
    //    headerDic.Add("zConSysId", "KII_CHA");
    //    headerDic.Add("zProSysId", "GRP_ECC_PP");
    //    headerDic.Add("zUserId", "bbs");
    //    headerDic.Add("zPiUser", "IF_KIICHA");
    //    headerDic.Add("zTimeId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
    //    headerDic.Add("zLang", "");

    //    return headerDic;
    //}

}
