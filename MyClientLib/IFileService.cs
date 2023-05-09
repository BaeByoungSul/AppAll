

using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System;
/// <summary>
/// DownloadFile: Server To Client로 File Stream 전송
/// UploadFile : Client To Server로 File Stream 전송
/// CheckFile : 서버에 파일이 있는지 ? 있어면 파일버전은 ?
/// Task가 필요한지 ?
/// </summary>
namespace Services.FileService
{
    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        UploadResponse UploadFile(UploadFile uploadFile);


        [OperationContract]
        DownloadResponse DownloadFile(DownloadFile request);

        [OperationContract]
        CheckFileResponse CheckFile(string fileName);

    }


    [MessageContract]
    public class UploadFile : IDisposable
    {

        [MessageHeader]
        public string FileName { get; set; } = string.Empty;

        [MessageHeader]
        public long FileLength { get; set; }

        [MessageBodyMember(Order = 2)]
        public Stream FileStream { get; set; }

        public void Dispose()
        {
            if (FileStream == null) return;
            FileStream.Close();
            FileStream.Dispose();
            FileStream = null;
            GC.SuppressFinalize(this);
        }
    }

    [MessageContract]
    public class UploadResponse
    {
        [MessageBodyMember]
        public string ReturnCD { get; set; }
        [MessageBodyMember]
        public string ReturnMsg { get; set; }


    }

    [MessageContract]
    public class DownloadFile
    {
        [MessageBodyMember]
        public string FileName { get; set; } = string.Empty;

    }

    [MessageContract]
    public class DownloadResponse
    {
        [MessageHeader]
        public string ReturnCD { get; set; }
        [MessageHeader]
        public string ReturnMsg { get; set; }

        [MessageHeader]
        public string FileName { get; set; } = string.Empty;

        [MessageHeader]
        public long FileLength { get; set; }

        [MessageBodyMember(Order = 1)]
        public Stream FileStream { get; set; }

        public void Dispose()
        {
            if (FileStream == null) return;
            FileStream.Close();
            FileStream.Dispose();
            FileStream = null;
            GC.SuppressFinalize(this);
        }
    }

    [DataContract]
    public class CheckFileResponse
    {
        [DataMember(Order = 0)]
        public bool FileExists { get; set; }

        [DataMember(Order = 1)]
        public string FileVersion { get; set; } = string.Empty;
    }
}






