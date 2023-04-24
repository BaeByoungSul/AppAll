// See https://aka.ms/new-console-template for more information
using Models.Database;
using Services.DbService;
using Services.FileService;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello, World!");
//RunTest.RunFileDownload("aaa.exe");
//RunTest.RunFileUpload("CRRedist2008_x64.msi");
//RunTest.RunGetDataSet();

RunTest.RunExecNonQuery();

public static class RunTest  {
    public static void RunGetDataSet() {
        // This code is written by an application developer.
        // Create a channel factory.
        //BasicHttpBinding myBinding = new BasicHttpBinding();

        BasicHttpBinding myBinding = GetDbHttpBinding();

        EndpointAddress myEndpoint = new EndpointAddress("http://172.20.105.36:7710/DBService");

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
        List<MyParaValue> paraValueList = new List<MyParaValue>();
        

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


        MyParaValue myParaValue = new();
        myParaValue.Add("@TEST_MST_NM", "안녕하세요(ttt)");

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
        List<MyParaValue> paraValueList = new List<MyParaValue>();


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


        MyParaValue myParaValue = new();
        myParaValue.Add("@TEST_DTL_NM", "배병술(ttt1)");
        myParaValue.Add("@AMOUNT", "123.45");
        paraValueList.Add(myParaValue);
        
        myParaValue = new();
        myParaValue.Add("@TEST_DTL_NM", "배병술(ttt2)");
        myParaValue.Add("@AMOUNT", "123.46");
        paraValueList.Add(myParaValue);

        myParaValue = new();
        myParaValue.Add("@TEST_DTL_NM", "배병술(ttt3)");
        myParaValue.Add("@AMOUNT", "123.47");
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
}

