
using Services.FileService;
using Models;
using System.Net;
using Models.Database;
using Services.DbService;
using Services.Sap;

var builder = WebApplication.CreateBuilder();

// Set up port (previously this was done in configuration,
// but CoreWCF requires it be done in code)
var hostConfig = builder.Configuration
        .GetSection("Hosting")
        .Get<Hosting>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.AllowSynchronousIO = true;
    options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB

    //options.Listen(IPAddress.Parse(hostConfig.Addr), int.Parse(hostConfig.PortHttp));
    //options.Listen(IPAddress.Parse(hostConfig.Addr),
    //            int.Parse(hostConfig.PortHttps), opt =>
    //            {
    //                opt.UseHttps(hostConfig.SslPath, hostConfig.SslPassword);
    //            });
    
    // DB Service Listen 
    options.Listen(IPAddress.Parse($"{hostConfig.Addr}"), int.Parse($"{hostConfig.DbHttpPort}"));

    // Sap Service Listen 
    options.Listen(IPAddress.Parse($"{hostConfig.Addr}"), int.Parse($"{hostConfig.SapHttpPort}"));

    // File Service Listen
    options.Listen(IPAddress.Parse($"{hostConfig.Addr}"), int.Parse($"{hostConfig.FileHttpPort}"));
});

//builder.WebHost.UseNetTcp(IPAddress.Parse($"{hostConfig.Addr}"), int.Parse($"{hostConfig.DbTcpPort}"));
builder.WebHost.UseNetTcp(IPAddress.Parse($"{hostConfig.Addr}"), int.Parse($"{hostConfig.FileTcpPort}"));


builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();


// configure DI for application services
var dbConnConfig = builder.Configuration
        .GetSection("DbConnectInfo")
        .Get<List<ConnectDBInfo>>();
builder.Services.AddSingleton(dbConnConfig);
var app = builder.Build();



app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<DbService>(serviceOptions =>
    {
        //serviceOptions.BaseAddresses.Add(baseAddr1);
        serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
    });

    var basicDbBinding = MyBinding.GetDbBasicBinding();
    var tcpDbBinding = MyBinding.GetDbTcpBinding();

    serviceBuilder.AddServiceEndpoint<DbService, IDbService>(
        basicDbBinding, 
        $"http://{hostConfig.Addr}:{hostConfig.DbHttpPort}/DBService"
    );



    //serviceBuilder.AddServiceEndpoint<DbService, IDbService>(
    //    tcpDbBinding,
    //    $"net.tcp://{hostConfig.Addr}:{hostConfig.DbTcpPort}/DBService"
    //);

    serviceBuilder.AddService<SapService>(serviceOptions =>
    {
        //serviceOptions.BaseAddresses.Add(baseAddr1);
        serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
    });

    var basicSapBinding = MyBinding.GetSapBasicBinding();
    
    serviceBuilder.AddServiceEndpoint<SapService, ISapService>(
        basicSapBinding,
        $"http://{hostConfig.Addr}:{hostConfig.SapHttpPort}/SapService"
    );



    var basicFileBinding = MyBinding.GetFileBasicBinding();
    var tcpFileBinding = MyBinding.GetFileTcpBinding();

    serviceBuilder.AddService<FileService>();
    serviceBuilder.AddServiceEndpoint<FileService, IFileService>(
        basicFileBinding,
        $"http://{hostConfig.Addr}:{hostConfig.FileHttpPort}/FileService"
    );
    serviceBuilder.AddServiceEndpoint<FileService, IFileService>(
        tcpFileBinding,
        $"net.tcp://{hostConfig.Addr}:{hostConfig.FileTcpPort}/FileService"
    );

    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    //serviceMetadataBehavior.HttpsGetEnabled = true;
    serviceMetadataBehavior.HttpGetEnabled = true;

});

app.Run();

class MyBinding
{
    public static BasicHttpBinding GetDbBasicBinding()
    {
        var basicBinding = new BasicHttpBinding();
        basicBinding.Security.Mode = BasicHttpSecurityMode.None;
        basicBinding.TransferMode = TransferMode.Streamed;
        basicBinding.MaxReceivedMessageSize = 2147483647;

        basicBinding.OpenTimeout = TimeSpan.FromMinutes(5);
        basicBinding.CloseTimeout = TimeSpan.FromMinutes(5);
        basicBinding.ReceiveTimeout = TimeSpan.FromMinutes(15);
        basicBinding.SendTimeout = TimeSpan.FromMinutes(15);

        return basicBinding;    

    }
    public static BasicHttpBinding GetSapBasicBinding()
    {
        var basicBinding = new BasicHttpBinding();
        basicBinding.Security.Mode = BasicHttpSecurityMode.None;
        basicBinding.TransferMode = TransferMode.Streamed;
        basicBinding.MaxReceivedMessageSize = 2147483647;

        basicBinding.OpenTimeout = TimeSpan.FromMinutes(5);
        basicBinding.CloseTimeout = TimeSpan.FromMinutes(5);
        basicBinding.ReceiveTimeout = TimeSpan.FromMinutes(15);
        basicBinding.SendTimeout = TimeSpan.FromMinutes(15);

        return basicBinding;

    }
    public static NetTcpBinding GetDbTcpBinding()
    {
        var nettcpBinding = new NetTcpBinding();
        nettcpBinding.TransferMode = TransferMode.Streamed;
        nettcpBinding.Security.Mode = SecurityMode.None;

        nettcpBinding.MaxReceivedMessageSize = 2147483647;
        nettcpBinding.OpenTimeout = TimeSpan.FromMinutes(5);
        nettcpBinding.CloseTimeout = TimeSpan.FromMinutes(5);
        nettcpBinding.ReceiveTimeout = TimeSpan.FromMinutes(15);
        nettcpBinding.SendTimeout = TimeSpan.FromMinutes(15);


        return nettcpBinding;

    }

    public static BasicHttpBinding GetFileBasicBinding ( )
    {
        var basicBinding = new BasicHttpBinding();
        basicBinding.TransferMode = TransferMode.Streamed;
        basicBinding.MessageEncoding = WSMessageEncoding.Mtom;
        basicBinding.MaxReceivedMessageSize = 2147483647;
        basicBinding.MaxBufferSize = 2147483647;

        //basicBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
        //basicBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
        //basicBinding.ReaderQuotas.MaxArrayLength = 2147483647;
        //basicBinding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
        //basicBinding.ReaderQuotas.MaxDepth = 2147483647;

        basicBinding.OpenTimeout = TimeSpan.FromMinutes(5);
        basicBinding.CloseTimeout = TimeSpan.FromMinutes(5);
        basicBinding.ReceiveTimeout = TimeSpan.FromMinutes(15);
        basicBinding.SendTimeout = TimeSpan.FromMinutes(15);

        return basicBinding;
    }
    public static NetTcpBinding GetFileTcpBinding()
    {
        var nettcpBinding = new NetTcpBinding();
        nettcpBinding.TransferMode = TransferMode.Streamed;

        nettcpBinding.Security.Mode = SecurityMode.None;
        nettcpBinding.MaxReceivedMessageSize = 2147483647;
        nettcpBinding.MaxBufferSize = 65536;

        nettcpBinding.OpenTimeout = TimeSpan.FromMinutes(5);
        nettcpBinding.CloseTimeout = TimeSpan.FromMinutes(5);
        nettcpBinding.ReceiveTimeout = TimeSpan.FromMinutes(15);
        nettcpBinding.SendTimeout = TimeSpan.FromMinutes(15);

        return nettcpBinding;
    }
}