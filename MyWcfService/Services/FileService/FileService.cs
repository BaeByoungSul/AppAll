using Models.File;
using System.Diagnostics;

namespace Services.FileService;

/// <summary>
/// UploadFile 
///   -. Stream Read는 테스트 결과 한번에 4096으로 고정됨
//       CopyTo 는 기본 버퍼크기가 81920 인데 한번에 얼마를 읽었는지 모르겠음.
/// </summary>
public class FileService : IFileService
{

    //private string serverPath = ConfigurationManager.AppSettings.Get("ServerFolder");

    private string? FileUploadRoot { get; set; } = string.Empty;

    public FileService()
    {
        var _configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false, reloadOnChange: true)
            .Build();


        FileUploadRoot = _configuration?.GetSection("Hosting:FileFolder").Value;
        if (FileUploadRoot == null) throw new Exception("Server Folder Error");

        //Console.WriteLine("File Service Created...{0}, {1}", GetClientAddress(), DateTime.Now);
    }
    private string? GetClientAddress()
    {
        // creating object of service when request comes   
        OperationContext context = OperationContext.Current;
        //Getting Incoming Message details   
        MessageProperties prop = context.IncomingMessageProperties;
        //Getting client endpoint details from message header   
        RemoteEndpointMessageProperty? endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
        return endpoint?.Address;
    }

    /// <summary>
    /// Stream as a return value in WCF - who disposes it?
    ///    ;; https://stackoverflow.com/questions/6483320/stream-as-a-return-value-in-wcf-who-disposes-it
    /// 서버에서 Steam을 Close해야 한다.    
    /// </summary>
    /// <param name="reqFile"></param>
    /// <returns></returns>
    /// <exception cref="FaultException"></exception>
    public DownloadResponse DownloadFile(DownloadFile reqFile)
    {
        try
        {
            //if (ServerFolder == null) throw new Exception("Server Folder Error");
            if (FileUploadRoot == null) throw new InvalidFileException("Server Folder Error");
            // 기본 directory가 없어면 생성
            if (!Directory.Exists(FileUploadRoot))
                throw new InvalidFileException("Server UplodFolder does not exists");




            Console.WriteLine(FileUploadRoot);
            string filePath = Path.Combine(FileUploadRoot, reqFile.FileName);

            //var fileData = new FileData();
            FileStream? stream = null;

            // 서버에서 Stream Close해야 함
            OperationContext clientContext = OperationContext.Current;


            stream = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite
                    );

            Console.WriteLine("DownloadFile Request {0} {1}", reqFile.FileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            var fileData = new DownloadResponse()
            {
                ReturnCD = "S",
                ReturnMsg = "Success",
                FileName = reqFile.FileName,
                FileLength = new FileInfo(filePath).Length,
                FileStream = stream
            };

            clientContext.OperationCompleted += (sender, args) =>
            {
                //Console.WriteLine("Download File Operation Completed");
                // stream?.Dispose();
                // stream= null;
                fileData?.Dispose();

            };


            return new DownloadResponse()
            {
                 ReturnCD="S",
                 ReturnMsg="Success",
                 FileName = reqFile.FileName,
                 FileLength=new FileInfo(filePath).Length,  
                 FileStream =stream
            };
        }
        catch (InvalidFileException ex)
        {
            return new DownloadResponse()
            {
                ReturnCD = "F",
                ReturnMsg = ex.Message,
                FileName = reqFile.FileName,
                FileLength = -1,
                FileStream = null
            };
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.ToString());

        }

    }

    public UploadResponse UploadFile(UploadFile uploadFile)
    {
        try
        {

            //if (uploadFile == null) throw new Exception("FileData is null error ");
            if (uploadFile.FileStream == null) throw new InvalidFileException("FileStream is null error ");
            if (FileUploadRoot == null) throw new InvalidFileException("Server UplodFolder setting error ");

            // 기본 directory가 없어면 생성
            if (!Directory.Exists(FileUploadRoot))
                throw new InvalidFileException("Server UplodFolder does not exists");


            // 서버 파일경로 + 파일명
            string filePath = Path.Combine(FileUploadRoot, uploadFile.FileName);

            // file이 있어면 삭제
            if (File.Exists(filePath)) File.Delete(filePath);

            //const int bufferLen = 65536;
            using (var targetStream =
                new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 65536))
            //using (var targetStream =
            //                new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Stream sourceStream = uploadFile.FileStream;

                // 기본 buffer size 4K
                sourceStream.CopyTo(targetStream);
                //byte[] buffer = new byte[bufferLen];
                //int count = 0;
                //while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                //{
                //    Debug.Print(count.ToString());
                //    // save to output stream
                //    targetStream.Write(buffer, 0, count);
                //}

                targetStream.Close();
                sourceStream.Close();
                Console.WriteLine("UploadFile Request {0} {1}", uploadFile.FileName, DateTime.Now);
                return new UploadResponse
                {
                    ReturnCD = "S",
                    ReturnMsg = "Success"

                };
                //return new UploadResponse
                //{ 
                //    MyReturn = new UploadResponse.ReturnBody
                //    {
                //        ReturnCD = "S",
                //        ReturnMsg = "Success"
                //    }
                    
                //};
            }

        }catch ( InvalidFileException ex)
        {
            return new UploadResponse
            {
                ReturnCD = "F",
                ReturnMsg = ex.Message

            };

            //return new UploadResponse
            //{
            //    MyReturn = new UploadResponse.ReturnBody
            //    {
            //        ReturnCD = "F",
            //        ReturnMsg = ex.Message
            //    }

            //};
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.ToString());
        }
    }

    public CheckFileResponse CheckFile(string fileName)
    {
        try
        {
            if (FileUploadRoot == null) throw new Exception("Server Folder Error");

            // 서버 파일경로 + 파일명
            string filePath = Path.Combine(FileUploadRoot, fileName);

            if (!File.Exists(filePath))
            {
                return new CheckFileResponse
                {
                    FileExists = false,
                    FileVersion = string.Empty
                };
            }
            else
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);

                // 버전이 null인 것은 공백처리
                return new CheckFileResponse
                {
                    FileExists = true,
                    FileVersion = string.IsNullOrEmpty(versionInfo.FileVersion) ? string.Empty : versionInfo.FileVersion
                };
            }
        }

        catch (Exception ex)
        {
            throw new FaultException(ex.ToString());
        }

    }

    
}