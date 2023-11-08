//using Mammoth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using Services.FileService;
using System.ComponentModel.DataAnnotations;

namespace MyWebApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{

    private readonly IWebHostEnvironment _env;
    private readonly IFileService _fileService;

    public FileController(IFileService fileService, IWebHostEnvironment environment)
    {
        _env = environment;
        _fileService = fileService;
    }

    private static string GetContentType( string fileName)
    {
        _ = new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string? contentType);
        return contentType ?? "application/octet-stream";
    }


    
    /// <summary>
    /// subdirectory path : dir1?dir2?dir3 늘 ?로 병합이 되어 argument로 넘어온다
    /// </summary>
    /// <returns></returns>
    [HttpPost("UploadFile"),
        DisableRequestSizeLimit,
        RequestFormLimits(MultipartBodyLengthLimit = Int32.MaxValue)]
    public async Task<IActionResult> UploadFile([Required] List<IFormFile> formFiles, string? subDirectory)
    {
        try
        {
            // 기본 directory가 없어면 생성
            if (!Directory.Exists(Path.Combine(_env.ContentRootPath, "Files")))
            {
                Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "Files"));
            }
            var serverPath = GetPathString(subDirectory);

            if (!Directory.Exists(serverPath))
            {
                return BadRequest("Server Directory Not Exists");
            }

            await _fileService.UploadFileAsync(formFiles, serverPath);
            return Ok(new { formFiles.Count, Size = SizeConverter(formFiles.Sum(f => f.Length)) });
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: ex.HResult.ToString());
            //return BadRequest(ex.Message);
        }
    }
    [HttpGet("DownloadFile"), DisableRequestSizeLimit]
    public async Task<IActionResult> DownloadFile([Required] string fileName, string? subDirectory)
    {
        try
        {
            var serverPath = GetPathString(subDirectory);

            string filePath = Path.Combine(serverPath, fileName);

            if (!System.IO.File.Exists(filePath))
                return BadRequest();

            var memory = await _fileService.DownloadFileAsync(fileName, serverPath);

            return File(memory, GetContentType(fileName), fileName); // attachment

            //return new FileStreamResult(memory, GetContentType(fileName))
            //{
            //    FileDownloadName = fileName
            //} ;

            //return File(memory, "application/msword", fileName); // attachment
            //return File(memory, "application/octet-stream"); // inline

        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: ex.HResult.ToString());
            //return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// return: file list in directory, sub directory list in directory
    ///         directory path : [dir1][dir2]
    /// </summary>
    /// <param name="subDirectory"></param>
    /// <returns></returns>
    [HttpGet("GetDirInfo")]
    public IActionResult GetDirInfo(string? subDirectory, string? fileFilter)
    {
        try
        {
            // 기본 directory가 없어면 생성
            if (!Directory.Exists(Path.Combine(_env.ContentRootPath, "Files")))
            {
                Directory.CreateDirectory(Path.Combine(_env.ContentRootPath, "Files"));
            }


            // Root dir과 합쳐진 서버경로를 만든다.
            var subDirs = subDirectory?.Split('?');
            string? fromRootDir = null; //= Path.Combine("Files");
            foreach (var item in subDirs ?? Enumerable.Empty<string>())
            {
                fromRootDir = Path.Combine(fromRootDir ?? string.Empty, item);
            }
            string serverDirectory = string.Empty;
            if (fromRootDir == null)
            {
                serverDirectory = Path.Combine(_env.ContentRootPath, "Files");
            }
            else
                serverDirectory = Path.Combine(_env.ContentRootPath, "Files", fromRootDir);

            //서버경로가 없어면 오류
            if (!Directory.Exists(serverDirectory))
            {
                return BadRequest("Server Directory Not Exists");
            }


            // ContentRootPath를 제외한 서버폴더 경로를 Array로 { dir1, dir2, ... }
            string[]? dirPathArray = fromRootDir?.Split(Path.DirectorySeparatorChar);


            // 서버폴더의 파일경로 및 파일명
            List<string> listFilePath = new List<string>();
            if (fileFilter.IsNullOrEmpty())
            {
                //filePaths = Directory.GetFiles(serverDirectory);
                listFilePath.AddRange(Directory.GetFiles(serverDirectory,"*.*"));
            }
            else
            {
                var searchPatterns = fileFilter?.Split(',');
                for (int i = 0; i < searchPatterns?.Length; i++)
                {
                    listFilePath.AddRange(Directory.GetFiles(serverDirectory, searchPatterns[i]));
                }
            }
            string[] filePaths = listFilePath.ToArray();


            // 해당 폴더의 파일목록
            var fileInfos = filePaths.Select(s => new
            {
                //var fileinfo = new FileInfo(s)
                fileName = Path.GetFileName(s),//s.Replace(sDir ?? string.Empty, ""),
                size = this.SizeConverter(new FileInfo(s).Length)
            });

            // 해당 폴더의 sub directory 목록
            var dirs = Directory.GetDirectories(serverDirectory);
            var subDirNames = dirs.Select(s => new
            {
                //var fileinfo = new FileInfo(s)
                dirName = Path.GetFileName(s),//s.Replace(sDir ?? string.Empty, ""),
                updated = new FileInfo(s).LastWriteTime
            });


            return Ok(new { fileInfos, subDirNames, dirPathArray });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteFile")]
    public IActionResult DeleteFile(string joinPaths)
    {
        var serverPath = GetPathString(joinPaths);

        if (!System.IO.File.Exists(serverPath))
        {
            return BadRequest("Server file Not Exists");
        }

        FileAttributes attr = System.IO.File.GetAttributes(serverPath);


        if (attr.HasFlag(FileAttributes.Directory))
        {
            //ClearFolder(serverPath);
            return BadRequest("This is directory");

        }
        else
        {
            System.IO.File.Delete(serverPath);
        }
        return Ok();
    }

    //[Authorize(Roles = "Admin1")]
    [HttpDelete("DeleteFoler")]
    public IActionResult DeleteFolder(string joinPaths)
    {
        var serverPath = GetPathString(joinPaths);

        if (!System.IO.Directory.Exists(serverPath))
        {
            return BadRequest("Server directory Not Exists");
        }

        FileAttributes attr = System.IO.File.GetAttributes(serverPath);
        if (attr.HasFlag(FileAttributes.Directory))
        {
            //ClearFolder(serverPath);
            Directory.Delete(serverPath, true);
        }
        else
        {
            return BadRequest("You request file delete");
        }
        return Ok();
    }

    [HttpPost("CreateFolder")]
    public IActionResult CreateFolder(string newFolder, string? joinPaths)
    {
        var serverPath = GetPathString(joinPaths);

        if (!System.IO.Directory.Exists(serverPath))
        {
            return BadRequest("Server directory Not Exists");
        }

        if (System.IO.Directory.Exists(Path.Combine(serverPath, newFolder)))
        {
            return BadRequest("Server directory already Exists");
        }
        System.IO.Directory.CreateDirectory(Path.Combine(serverPath, newFolder));


        return Ok();
    }


    private string SizeConverter(long bytes)
    {
        var fileSize = new decimal(bytes);
        var kilobyte = new decimal(1024);
        var megabyte = new decimal(1024 * 1024);
        var gigabyte = new decimal(1024 * 1024 * 1024);

        switch (fileSize)
        {
            case var _ when fileSize < kilobyte:
                return $"Less then 1KB";
            case var _ when fileSize < megabyte:
                return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";
            case var _ when fileSize < gigabyte:
                return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";
            case var _ when fileSize >= gigabyte:
                return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";
            default:
                return "n/a";
        }
    }
    private string GetPathString(string? joinPaths)
    {
        var splitPaths = joinPaths?.Split('?');
        //var splitPaths = joinPaths?.Split(':');

        string? fromRootPath = null;
        foreach (string path in splitPaths ?? Enumerable.Empty<string>())
        {
            fromRootPath = Path.Combine(fromRootPath ?? string.Empty, path);
        }
        string serverPath = string.Empty;
        if (fromRootPath == null)
        {
            serverPath = Path.Combine(_env.ContentRootPath, "Files");
        }
        else
            serverPath = Path.Combine(_env.ContentRootPath, "Files", fromRootPath);

        return serverPath;
    }


    //[HttpGet("GetDocHtml")]
    //public IActionResult GetDocHtml([Required] string fileName, string? subDirectory)
    //{
    //    var serverPath = GetPathString(subDirectory);

    //    string filePath = Path.Combine(serverPath, fileName);

    //    if (!System.IO.File.Exists(filePath)) return BadRequest();
    //    //return base.Content("<p>Not Found</p>", "text/html"); ;

    //    var converter = new DocumentConverter();
    //    var result = converter.ConvertToHtml(filePath);
    //    var html = result.Value; // The generated HTML
    //    var warnings = result.Warnings; // Any warnings during conversion
    //    Console.WriteLine(warnings);
    //    return base.Content(html, "text/html");
    //}
    #region Test 

    [HttpGet("GetPdf")]
    public async Task<IActionResult> GetPdfAsync([Required] string fileName, string? subDirectory)
    {
        var serverPath = GetPathString(subDirectory);

        string filePath = Path.Combine(serverPath, fileName);

        if (!System.IO.File.Exists(filePath))
            return BadRequest();

        var ms = await _fileService.DownloadFileAsync(fileName, serverPath);
        //return new FileStreamResult(ms, "application/pdf");


        return new FileStreamResult(ms, GetContentType(fileName));

        //var bytes = System.IO.File.ReadAllBytes(filePath);
        //return new FileContentResult(bytes, "application/pdf");
    }
    [HttpGet("GetDoc2")]
    public async Task<IActionResult> GetDoc2Async([Required] string fileName, string? subDirectory)
    {
        var serverPath = GetPathString(subDirectory);

        string filePath = Path.Combine(serverPath, fileName);

        if (!System.IO.File.Exists(filePath))
            return BadRequest();

        //var ms = await _fileService.DownloadFileAsync(fileName, serverPath);

        //return new FileStreamResult(ms, GetContentType(fileName))  ;

        byte[] bytes = System.IO.File.ReadAllBytes(filePath );
        return File(bytes, GetContentType(fileName),true );

    }

    [HttpGet("GetDoc")]
    public async Task<IActionResult> GetDoc()
    {
        var cd = new System.Net.Mime.ContentDisposition
        {
            FileName = "bbb.docx",

            // always prompt the user for downloading, set to true if you want 
            // the browser to try to show the file inline
            Inline = false,
        };
        //Response.AppendHeader("Content-Disposition", cd.ToString());
        
        var sourcetDir = Path.Combine(_env.ContentRootPath, "Files");
        string filePath = sourcetDir + "/" + "bbb.docx";

        var stream = new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite);
        return new FileStreamResult(stream, GetContentType("bbb.docx"));

    }

  
    [HttpPost("UploadTest")]
    public IActionResult UploadTest([Required] List<IFormFile> formFiles, string subDirectory)
    {
        try
        {
            Console.WriteLine(subDirectory);
            //_fileService.UploadFile(formFiles, subDirectory);
            foreach (var item in formFiles)
            {
                Console.WriteLine(item.FileName);
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion Test

}
public class DocxModel
{
    public byte[] testDocx { get; set; }
}