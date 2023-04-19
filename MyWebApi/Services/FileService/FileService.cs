

namespace Services.FileService;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment environment) 
    {
        _env = environment;
    }
    public async Task UploadFileAsync(List<IFormFile> formFiles, string? subDirectory)
    {
        try
        {
            var targetDir = Path.Combine(_env.ContentRootPath, "Files", subDirectory ?? String.Empty);

            foreach (var formFile in formFiles)
            {
                if (formFile.Length > 0)
                {
                    //var filePath = Path.GetTempFileName();
                    var filePath = targetDir + "/" + formFile.FileName;

                    // file이 있어면 삭제
                    if (File.Exists(filePath)) File.Delete(filePath);

                    using (var stream = new FileStream(
                            filePath,
                            FileMode.Create,
                            FileAccess.Write,
                            FileShare.None))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
        

    }
    public async Task<MemoryStream> DownloadFileAsync(string fileName, string? subDirectory)
    {
        try
        {
            var sourcetDir = Path.Combine(_env.ContentRootPath, "Files", subDirectory ?? String.Empty);
            string filePath = sourcetDir + "/" + fileName;

            var memory = new MemoryStream();
            using (var stream = new FileStream(
                        filePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.ReadWrite))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return memory;
        }
        catch (Exception)
        {

            throw;
        }
       
    }

    //public string SizeConverter(long bytes)
    //{
    //    var fileSize = new decimal(bytes);
    //    var kilobyte = new decimal(1024);
    //    var megabyte = new decimal(1024 * 1024);
    //    var gigabyte = new decimal(1024 * 1024 * 1024);

    //    switch (fileSize)
    //    {
    //        case var _ when fileSize < kilobyte:
    //            return $"Less then 1KB";
    //        case var _ when fileSize < megabyte:
    //            return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";
    //        case var _ when fileSize < gigabyte:
    //            return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";
    //        case var _ when fileSize >= gigabyte:
    //            return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";
    //        default:
    //            return "n/a";
    //    }
    //}

    //public async Task UploadFileAsync(List<IFormFile> files, string? subDirectory)
    //{
    //    var targetDir = Path.Combine(_env.ContentRootPath, "Files", subDirectory ?? String.Empty);

    //    //long size = files.Sum(f => f.Length);

    //    foreach (var formFile in files)
    //    {
    //        if (formFile.Length > 0)
    //        {
    //            //var filePath = Path.GetTempFileName();
    //            var filePath = targetDir + "/" + formFile.FileName;

    //            // file이 있어면 삭제
    //            if (File.Exists(filePath)) File.Delete(filePath);

    //            using (var stream = new FileStream(
    //                    filePath, 
    //                    FileMode.Create, 
    //                    FileAccess.Write, 
    //                    FileShare.None))
    //            {
    //                await formFile.CopyToAsync(stream);
    //            }
    //        }
    //    }


    //}

}
