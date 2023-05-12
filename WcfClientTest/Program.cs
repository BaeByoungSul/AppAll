// See https://aka.ms/new-console-template for more information
using Models.Database;

using Services.DbService;
using Services.FileService;
using System.Data;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

using Sap;
using Sap.PP0370;
using Sap.PP0060;
using System;
using Models.Sap;
using Services.Sap;

Console.WriteLine("Hello, World!");
// RunTest.RunFileDownload("aaa.exe");

//RunTest.RunFileUpload("CRRedist2008_x64.msi");
//RunTest.RunGetDataSet();

//RunTest.RunExecNonQuery();
//RunTest.RunPP0370();

//RunTest.RunPP0060();
RunTest.RunSapReq();
public static class RunTest  {
    private static readonly string PP0370_QAS = "http://infheaidrdb01.kolon.com:51000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_QAS&receiverParty=&receiverService=&interface=SI_GRP_PP0370_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp";
    private static readonly string PP0060_QAS = "http://infheaidrdb01.kolon.com:51000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_QAS&receiverParty=&receiverService=&interface=SI_GRP_PP0060_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp\r\n";

    public static   void RunSapReq()
    {
        BasicHttpBinding myBinding = GetDbHttpBinding();

        EndpointAddress myEndpoint = new EndpointAddress("http://172.20.105.36:6310/SapService");

        ChannelFactory<ISapService> myChannelFactory = new ChannelFactory<ISapService>(myBinding, myEndpoint);

        // Create a channel.
        ISapService _cli = myChannelFactory.CreateChannel();

        string url = "http://infheaidrdb01.kolon.com:51000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_QAS&receiverParty=&receiverService=&interface=SI_GRP_PP0370_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp";
        string req = PP0370_REQ();
        SapRequest cmd = new SapRequest()
        {
            RequestUrl = url,
            RequestXml = req

        };
        try
        {
            var rtn =  _cli.Request_Sap(cmd).Result;
            //        Console.WriteLine(rtn.ReturnCD);
            //rtn.RtnBody.RemoveAttribute("xmlns"); // StringReader할 때 오류가 나서 추가함

            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable("Header");
            dt1.Columns.Add("zResultCd", typeof(string));
            dt1.Columns.Add("zResultMsg", typeof(string));
            ds.Tables.Add(dt1);

            DataTable dt2 = new DataTable("STOCK_LIST");
            dt2.Columns.Add("WERKS", typeof(string));
            dt2.Columns.Add("MATNR", typeof(string));
            ds.Tables.Add(dt2);


            // Console.WriteLine(rtn.FirstChild);
            // var rtnHeader = rtn.FirstChild as XmlElement;

            Console.WriteLine(rtn.OuterXml);
            StringReader theReader = new StringReader(rtn.OuterXml );
            ds.ReadXml(theReader, XmlReadMode.IgnoreSchema);

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }



        ((IClientChannel)_cli).Close();

        myChannelFactory.Close();
    }

    private static string PP0370_REQ()
    {

        Dictionary<string, string> headerDic = GetHeaderDic();


        StringBuilder sb = new StringBuilder();
        StringWriter strw = new StringWriter(sb);

        using (XmlTextWriter w = new XmlTextWriter(strw))
        {
            w.Formatting = Formatting.Indented;

            w.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            w.WriteAttributeString("xmlns", "inf", null, "http://grpeccpp.esp.com/infesp");

            w.WriteStartElement("soapenv", "Header", null);
            w.WriteEndElement(); // End Of soapenv:Header

            w.WriteStartElement("soapenv", "Body", null);
            w.WriteStartElement("inf", "MT_GRP_PP0370_Con", null);

            // 공통부분
            w.WriteStartElement("Header");
            {
                foreach (KeyValuePair<string, string> di in headerDic)
                {
                    w.WriteElementString(di.Key, di.Value);
                }

            }
            w.WriteEndElement(); // End Of Header
                                 // 공통부분 끝


            w.WriteStartElement("Body");
            {

                w.WriteStartElement("T_WERKS");
                w.WriteElementString("WERKS", "5131");
                w.WriteEndElement();

                w.WriteStartElement("T_MATNR");
                w.WriteElementString("MATNR", "10352688");
                w.WriteEndElement();

                w.WriteStartElement("T_LGORT");
                w.WriteElementString("LGORT", "5701");
                w.WriteEndElement();

            }

            w.WriteEndElement(); // End Of Body


            w.WriteEndElement(); // End Of inf:MT_GRP_PP0370_Con
            w.WriteEndElement(); // End Of soapenv:Body
            w.WriteEndElement(); // End Of First Start
            w.Close();

        }


        return sb.ToString();
        //Console.WriteLine(strw.ToString());

        //XmlDocument xmlDoc = new XmlDocument();
        //xmlDoc.LoadXml(sb.ToString());
        //return xmlDoc;
    }
    private static Dictionary<string, string> GetHeaderDic()
    {
        Dictionary<string, string> headerDic = new Dictionary<string, string>();
        headerDic.Add("zInterfaceId", "GRP_PP0100");
        headerDic.Add("zConSysId", "KII_CHA");
        headerDic.Add("zProSysId", "GRP_ECC_PP");
        headerDic.Add("zUserId", "bbs");
        headerDic.Add("zPiUser", "IF_KIICHA");
        headerDic.Add("zTimeId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        headerDic.Add("zLang", "");

        return headerDic;
    }
    public static void RunPP0060()
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

        var rtn =  _cli.SI_GRP_PP0060_SO(request);
       // Console.WriteLine(rtn);

        //rtn.RtnBody.RemoveAttribute("xmlns"); // StringReader할 때 오류가 나서 추가함
        //Console.WriteLine(rtn.RtnBody?.OuterXml);
        //StringReader theReader = new StringReader(rtn.RtnBody.OuterXml);

        //DataSet ds = new DataSet();
        //ds.ReadXml(theReader, XmlReadMode.ReadSchema);


        ((IClientChannel)_cli).Close();

        cf.Close();
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
             MT_GRP_PP0060_Con =  new DT_GRP_PP0060_Con()
             {
                 Header = reqHeader,    
                 Body = new DT_PP0060_ReqBody()
                 {
                      WERKS= "5131",
                      SPMON=  "202301"
                 }
             }
        };

        return req;

    }
   
    public static void RunPP0370()
    {
        // This code is written by an application developer.
        // Create a channel factory.
        //BasicHttpBinding myBinding = new BasicHttpBinding();
        PP0370_Request request = Get_Req_PP0370();

        //Console.WriteLine(XmlSerialize(typeof(SI_GRP_PP0370_SORequest),
        //                request, null));

        BasicHttpBinding myBinding = GetSapHttpBinding();

        EndpointAddress myEndpoint = new EndpointAddress(PP0370_QAS);

        ChannelFactory<ISapPP0370> cf = new ChannelFactory<ISapPP0370>(myBinding, myEndpoint);
        cf.Credentials.UserName.UserName = "IF_KIICHA";
        cf.Credentials.UserName.Password = "Interface!12";
        // Create a channel.
        ISapPP0370 _cli = cf.CreateChannel();

        var rtn = _cli.SI_GRP_PP0370_SO(request);
        Console.WriteLine(rtn);

        //rtn.RtnBody.RemoveAttribute("xmlns"); // StringReader할 때 오류가 나서 추가함
        //Console.WriteLine(rtn.RtnBody?.OuterXml);
        //StringReader theReader = new StringReader(rtn.RtnBody.OuterXml);

        //DataSet ds = new DataSet();
        //ds.ReadXml(theReader, XmlReadMode.ReadSchema);


        ((IClientChannel)_cli).Close();

        cf.Close();
    }
    private static PP0370_Request Get_Req_PP0370()
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

        CWERKS[] werks = new CWERKS[]
        { new CWERKS {WERKS = "1108"}
          };
        CMATNR[] matnrs = new CMATNR[]
        { new CMATNR  {MATNR = "10004001"} };

        CLGORT[] lgorts = new CLGORT[]
        {
                new CLGORT { LGORT="3101"},
                new CLGORT { LGORT="1101"}

        };
        // req para Body 
        DT_ReqBody reqBody = new DT_ReqBody()
        {
            T_WERKS = werks,
            T_MATNR = matnrs,
            T_LGORT = lgorts
        };
        DT_GRP_PP0370_Con mt = new DT_GRP_PP0370_Con()
        {
            Header = reqHeader,
            Body = reqBody
        };
        // mt.Header = reqHeader;

        //private DT_ReqHeader headerField;
        // private DT_GRP_PP0370_ConBody bodyField;
        PP0370_Request req = new  PP0370_Request(mt);

        return req;

    }
    public static void RunGetDataSet() {
        // This code is written by an application developer.
        // Create a channel factory.
        //BasicHttpBinding myBinding = new BasicHttpBinding();

        BasicHttpBinding myBinding = GetDbHttpBinding();

        EndpointAddress myEndpoint = new EndpointAddress("http://172.20.105.36:5110/DBService");

        ChannelFactory<IDbService> myChannelFactory = new ChannelFactory<IDbService>(myBinding, myEndpoint);

        // Create a channel.
        IDbService _cli = myChannelFactory.CreateChannel();

        MyCommand cmd = new MyCommand() { 
            CommandName="MST",
            ConnectionName="BSBAE",
            CommandType=1,
            CommandText= "SELECT * FROM TESTDB..TEST_MST;SELECT * FROM TESTDB..TEST_DTL"

        };
        var rtn = _cli.GetDataSet(cmd);
        Console.WriteLine(rtn.ReturnCD);

        //rtn.RtnBody.RemoveAttribute("xmlns"); // StringReader할 때 오류가 나서 추가함
        Console.WriteLine(rtn.RtnBody?.OuterXml);
        StringReader theReader = new StringReader(rtn.RtnBody.OuterXml);

        DataSet ds = new DataSet();
        ds.ReadXml(theReader, XmlReadMode.ReadSchema);


        ((IClientChannel)_cli).Close();

        myChannelFactory.Close();
    }
    public static void RunExecNonQuery()
    {
        // This code is written by an application developer.
        // Create a channel factory.
        //BasicHttpBinding myBinding = new BasicHttpBinding();

        BasicHttpBinding myBinding = GetDbHttpBinding();

        EndpointAddress myEndpoint = new EndpointAddress("http://172.20.105.36:7710/DBService");

        ChannelFactory<IDbService> myChannelFactory = new ChannelFactory<IDbService>(myBinding, myEndpoint);

        // Create a channel.
        IDbService _cli = myChannelFactory.CreateChannel();

        List<MyCommand> cmds = new List<MyCommand>();
        cmds.Add(CreateCommandHdr());
        cmds.Add(CreateCommandDtl());

        var rtn = _cli.ExecNonQuery(cmds);
        Console.WriteLine(rtn.ReturnCD);

        //rtn.RtnBody.RemoveAttribute("xmlns"); // StringReader할 때 오류가 나서 추가함
        Console.WriteLine(rtn.RtnBody?.OuterXml);
        StringReader theReader = new StringReader(rtn.RtnBody.OuterXml);

        DataSet ds = new DataSet();
        ds.ReadXml(theReader, XmlReadMode.ReadSchema);

        ((IClientChannel)_cli).Close();

        myChannelFactory.Close();
    }
    private static MyCommand CreateCommandHdr()
    {
        MyCommand cmd = new MyCommand()
        {
            CommandName = "MST",
            ConnectionName = "BSBAE",
            CommandType = 4,
            CommandText = "TESTDB..USP_TEST_MST_INS"
        };

        List<MyPara> paraList = new List<MyPara>();
        List<MyParaValue[]> paraValueList = new List<MyParaValue[]>();
        

        MyPara myPara = new()
        {
            ParameterName = "@TEST_MST_NM",
            DbDataType = (int)SqlDbType.VarChar
        };
        paraList.Add(myPara);
        myPara = new()
        {
            ParameterName = "@TEST_ID",
            DbDataType = (int)SqlDbType.BigInt,
            Direction = (int)ParameterDirection.Output
        };
        paraList.Add(myPara);
        cmd.Parameters = paraList;


        MyParaValue[] myParaValue= new MyParaValue[1];
        myParaValue[0].ParameterName = "@TEST_MST_NM";
        myParaValue[0].ParameterValue = "안녕하세요(ttt)";

        //new() { 
        //ParameterName = "@TEST_MST_NM",
        //ParameterValue ="안녕하세요(ttt)"};

        paraValueList.Add(myParaValue);

        cmd.ParaValues = paraValueList; 

        return cmd;
    }
    private static MyCommand CreateCommandDtl()
    {
        MyCommand cmd = new MyCommand()
        {
            CommandName = "DTL",
            ConnectionName = "BSBAE",
            CommandType = 4,
            CommandText = "TESTDB..USP_TEST_DTL_INS"
        };

        List<MyPara> paraList = new List<MyPara>();
        List<MyParaValue[]> paraValueList = new List<MyParaValue[]>();


        MyPara myPara = new()
        {
            ParameterName = "@TEST_ID",
            DbDataType = (int)SqlDbType.BigInt,
            HeaderCommandName="MST",
            HeaderParameter="@TEST_ID"
        };
        paraList.Add(myPara);
        myPara = new()
        {
            ParameterName = "@TEST_DTL_NM",
            DbDataType = (int)SqlDbType.VarChar,
        };
        paraList.Add(myPara);
        myPara = new()
        {
            ParameterName = "@AMOUNT",
            DbDataType = (int)SqlDbType.Decimal
        };
        paraList.Add(myPara);

        cmd.Parameters = paraList;


        MyParaValue[] myParaValue = new MyParaValue[2] ;
        myParaValue[0].ParameterName = "@TEST_DTL_NM";
        myParaValue[0].ParameterValue = "배병술(ttt2)";
        myParaValue[1].ParameterName = "@AMOUNT";
        myParaValue[1].ParameterValue = "123.45";

        paraValueList.Add(myParaValue);
        
        myParaValue = new MyParaValue[2];
        myParaValue[0].ParameterName = "@TEST_DTL_NM";
        myParaValue[0].ParameterValue = "배병술(ttt3)";
        myParaValue[1].ParameterName = "@AMOUNT";
        myParaValue[1].ParameterValue = "123.46";
        paraValueList.Add(myParaValue);

        myParaValue = new MyParaValue[2];
        myParaValue[0].ParameterName = "@TEST_DTL_NM";
        myParaValue[0].ParameterValue = "배병술(ttt4)";
        myParaValue[1].ParameterName = "@AMOUNT";
        myParaValue[1].ParameterValue = "123.47";
        paraValueList.Add(myParaValue);


        cmd.ParaValues = paraValueList;

        return cmd;
    }

    public static void RunFileDownload(string fileName)
    {
        // This code is written by an application developer.
        // Create a channel factory.
        //BasicHttpBinding myBinding = new BasicHttpBinding();

        BasicHttpBinding myBinding = GetFileHttpBinding();

        EndpointAddress myEndpoint = new EndpointAddress("http://172.20.105.36:7750/FileService");

        ChannelFactory<IFileService> myChannelFactory = new ChannelFactory<IFileService>(myBinding, myEndpoint);

        // Create a channel.
        IFileService _cli = myChannelFactory.CreateChannel();

        DownloadFile downloadFile = new DownloadFile()
        {
            FileName = fileName
        };
        var rtn = _cli.DownloadFile(downloadFile);
        Console.WriteLine(rtn.ReturnCD);

        string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName;

        // 기존 파일 삭제 후 아래에서 다운로드
        if(File.Exists(filePath))  File.Delete(filePath);

        FileStream targetStream = File.Create(filePath);
        rtn.FileStream?.CopyTo(targetStream);
        targetStream.Close();
        
        ((IClientChannel)_cli).Close();

        // Create another channel.
        //IFileService wcfClient2 = myChannelFactory.CreateChannel();
        //s = wcfClient2.Add(15, 27);
        //Console.WriteLine(s.ToString());
        //((IClientChannel)wcfClient2).Close();
        myChannelFactory.Close();
    }
    public static void RunFileUpload(string fileName)
    {
        // This code is written by an application developer.
        // Create a channel factory.
        //BasicHttpBinding myBinding = new BasicHttpBinding();

        BasicHttpBinding myBinding = GetFileHttpBinding();

        EndpointAddress myEndpoint = new EndpointAddress("http://172.20.105.36:7750/FileService");

        ChannelFactory<IFileService> myChannelFactory = new ChannelFactory<IFileService>(myBinding, myEndpoint);

        // Create a channel.
        IFileService _cli = myChannelFactory.CreateChannel();

        //CRRedist2008_x64.msi
        string filepath = @"D:\Software\"+ fileName;

        using (System.IO.FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
        {
            var uploadfile = new UploadFile()
            {
                FileName = fileName,
                FileLength = new FileInfo(filepath).Length,
                FileStream = stream
            };
            var rtn = _cli.UploadFile(uploadfile);
            Console.WriteLine(rtn.ReturnCD);
            //clientUpload.UploadFile(stream);
        }

        ((IClientChannel)_cli).Close();

        // Create another channel.
        //IFileService wcfClient2 = myChannelFactory.CreateChannel();
        //s = wcfClient2.Add(15, 27);
        //Console.WriteLine(s.ToString());
        //((IClientChannel)wcfClient2).Close();
        myChannelFactory.Close();
    }
    
    private static BasicHttpBinding GetFileHttpBinding()
    {
        BasicHttpBinding binding = new BasicHttpBinding();

        binding.TransferMode = TransferMode.Streamed;
        binding.MessageEncoding = WSMessageEncoding.Mtom;
        binding.Security.Mode = BasicHttpSecurityMode.None;

        binding.MaxReceivedMessageSize = 2147483647;
        binding.MaxBufferSize = 65536;

        binding.ReaderQuotas.MaxStringContentLength = 2147483647;
        binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
        binding.ReaderQuotas.MaxArrayLength = 2147483647;
        binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
        binding.ReaderQuotas.MaxDepth = 2147483647;

        binding.OpenTimeout = TimeSpan.FromMinutes(5);
        binding.CloseTimeout = TimeSpan.FromMinutes(5);
        binding.ReceiveTimeout = TimeSpan.FromMinutes(15);
        binding.SendTimeout = TimeSpan.FromMinutes(15);

        return binding;
    }
    private static BasicHttpBinding GetDbHttpBinding()
    {
        BasicHttpBinding binding = new BasicHttpBinding();

        binding.TransferMode = TransferMode.Streamed;
        //binding.MessageEncoding = WSMessageEncoding.Mtom;
        binding.Security.Mode = BasicHttpSecurityMode.None;

        binding.MaxBufferSize = 2147483647;
        binding.MaxReceivedMessageSize = 2147483647;

        binding.OpenTimeout = TimeSpan.FromMinutes(5);
        binding.CloseTimeout = TimeSpan.FromMinutes(5);
        binding.ReceiveTimeout = TimeSpan.FromMinutes(15);
        binding.SendTimeout = TimeSpan.FromMinutes(15);

        binding.ReaderQuotas.MaxStringContentLength = 2147483647;

        return binding;
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

    public static string XmlSerialize(Type dataType, object data, XmlSerializerNamespaces xmlns)
    {

        XmlSerializer xmlSerializer = new XmlSerializer(dataType);
        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            OmitXmlDeclaration = true,
        };

        using (StringWriter textWriter = new StringWriter())
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
            {
                xmlSerializer.Serialize(xmlWriter, data, xmlns);
            }

            return textWriter.ToString(); //This is the output as a string

        }
        //var builder = new StringBuilder();
        //using (var writer = XmlWriter.Create(builder, settings))
        //{
        //    xmlSerializer.Serialize(writer, data, xmlns);
        //}
        //return builder.ToString();

    }

}

