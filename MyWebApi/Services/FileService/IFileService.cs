
namespace Services.FileService;
public interface IFileService
{
    Task UploadFileAsync(List<IFormFile> formFiles, string? subDirectory);
    Task<MemoryStream> DownloadFileAsync(string fileName, string? subDirectory);

//    Task UploadFileAsync(IFormFileCollection files, string? subDirectory);

}
