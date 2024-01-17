//using Mammoth;
using Microsoft.AspNetCore.Authorization;
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
            var serverPath = GetDirPathString(subDirectory);

            //if (!Directory.Exists(serverPath))
            //{
            //    return BadRequest("Server Directory Not Exists");
            //}

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
            var serverPath = GetDirPathString(subDirectory);

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

    /// <summary>
    /// 1. 오류 처리
    ///   >> 파라미터가 공백일 경우 
    /// </summary>
    /// <param name="queryDirPath"></param>
    /// <param name="fileFilter"></param>
    /// <returns></returns>
    [HttpGet("QueryInFolder")]
    public IActionResult QueryInFolder( string? queryDirJoin, string? fileFilterJoin)
    {
        try
        {
            
            string[]? fileFilter = fileFilterJoin?.Split("?");

            // 서버폴더
            string serverDirectory = GetDirPathString(queryDirJoin);
        

            //서버경로가 없어면 오류
            if (string.IsNullOrEmpty( serverDirectory))
                return BadRequest("Server Directory is null or empty");

            if (!Directory.Exists(serverDirectory))
                return BadRequest("Server Directory Not Exists");
            

            // file search 패턴이 있을 경우 한번에 처리할 방법이 없어서 패턴 별로 파일을 찾차서 List에 추가함
            List<string> filesPathList = new List<string>();
            if (fileFilter == null || fileFilter.Length <= 0)
            {
                filesPathList.AddRange(Directory.GetFiles(serverDirectory, "*.*"));
            }
            else
            {
                foreach (var item in fileFilter ?? Enumerable.Empty<string>())
                {
                    filesPathList.AddRange(Directory.GetFiles(serverDirectory, item));
                }
            }

            // 파일정보 생성: 파일 경로에서 파일명 및 파일 크기 조회
            var fileList = filesPathList.Select(s => new
            {
                fileName = Path.GetFileName(s),
                fileSize = this.SizeConverter(new FileInfo(s).Length)
            });

            
            // 해당 폴더의 폴더 목록
            var dirs = Directory.GetDirectories(serverDirectory);
            var dirList = dirs.Select(s => new
            {
                dirName = Path.GetFileName(s),
                updated = new FileInfo(s).LastWriteTime
            });

            string[]? queryDirPath = queryDirJoin?.Split("?");

            // 반환 값: 조회 한 서버 폴더, 파일목록, 폴더목록
            return Ok(new { queryDirPath, fileList, dirList });
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("CreateFolder")]
    [Authorize(Roles = "FileAdmin")]
    public IActionResult CreateFolder(string newFolder, string? serverDirJoin)
    {
        try
        {
            var serverPath = GetDirPathString(serverDirJoin);
            
            if (System.IO.Directory.Exists(Path.Combine(serverPath, newFolder)))
            {
                return BadRequest("Server directory already Exists");
            }
            System.IO.Directory.CreateDirectory(Path.Combine(serverPath, newFolder));

            return QueryInFolder(serverDirJoin, null);
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }

        //return Ok();
    }


    [HttpDelete("DeleteFile")]
    [Authorize(Roles = "FileAdmin")]
    public IActionResult DeleteFile(string sourceFileName, string? queryDirJoin)
    {
        try
        {
            string serverBackUpPath = Path.Combine(_env.ContentRootPath, "Files_backup");
            if (!System.IO.Directory.Exists(serverBackUpPath))
                throw new Exception("Server Backup Directory Not Exists");
            string bakfilePath = Path.Combine(serverBackUpPath, sourceFileName + '.' + DateTime.Now.ToString("yyyyMMddHHmmss"));


            var serverDirPath = GetDirPathString(queryDirJoin);
            var serverFilePath = Path.Combine(serverDirPath, sourceFileName);
            if (!System.IO.File.Exists(serverFilePath))
            {
                return BadRequest("Server file Not Exists");
            }

            System.IO.File.Move(serverFilePath, bakfilePath);

            //System.IO.File.Delete(serverFilePath);

            return QueryInFolder(queryDirJoin, null);
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpPut("RenameFile")]
    [Authorize(Roles = "FileAdmin")]
    public IActionResult RenameFile(string sourceFileName, string moveFileName, string? queryDirJoin  )
    {
        if(string.IsNullOrEmpty(sourceFileName))
            return BadRequest("Source file name is empty");

        if(string.IsNullOrEmpty(moveFileName))
            return BadRequest("Rename file is empty");

        if ( sourceFileName.Equals(moveFileName))
            return BadRequest("Source file name is equal to rename file");

        try
        {
            var serverDirPath = GetDirPathString(queryDirJoin);
            var serverFilePath = Path.Combine(serverDirPath, sourceFileName);
            if (!System.IO.File.Exists(serverFilePath))
                return BadRequest("Server file Not Exists");
            
            var moveFilePath = Path.Combine(serverDirPath, moveFileName);
            if (System.IO.File.Exists(moveFilePath))
                return BadRequest("Rename file is already Exists");
            
            // Create a FileInfo  
            System.IO.FileInfo fi = new System.IO.FileInfo(serverFilePath);
            // Check if file is there  
            if (fi.Exists)
            {
                // Move file with a new name. Hence renamed.  
                fi.MoveTo(moveFilePath);

                Console.WriteLine("File Renamed.");
                //    System.IO.File.Delete(serverPath);
            }
            return QueryInFolder(queryDirJoin, null);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
            
        }

    }

    
    [HttpDelete("DeleteFolder")]
    [Authorize(Roles = "FileAdmin")]
    public IActionResult DeleteFolder(string deleteFolder, string? queryDirJoin  )
    {
        try
        {
            string serverBackUpPath = Path.Combine(_env.ContentRootPath, "Files_backup");
            if (!System.IO.Directory.Exists(serverBackUpPath))
                throw new Exception("Server Backup Directory Not Exists");

            string bakDirPath = Path.Combine(serverBackUpPath, deleteFolder + '_' + DateTime.Now.ToString("yyyyMMddHHmmss"));


            var serverPath = GetDirPathString(queryDirJoin);
            var deletePath = Path.Combine(serverPath, deleteFolder);

            Directory.Move(deletePath, bakDirPath);
            //Directory.Delete(deletePath, true);

     
            return QueryInFolder(queryDirJoin, null);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

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
    private string GetPathString_bak(string? joinPaths)
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

    /// <summary>
    /// 해당 폴더가 있는지 점검
    /// </summary>
    /// <param name="queryDirPath"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private string GetDirPathString(string? queryDirPath)
    {
        var splitPaths = queryDirPath?.Split('?');
        
        // 조회할 폴더 파라미터가 공백일 경우 오류
        foreach (var item in splitPaths ?? Enumerable.Empty<string>())
        {
            if (string.IsNullOrEmpty(item))
                throw new Exception ("Server Directory is empty");
        }

        string? fromRootPath = null;
        foreach (string path in splitPaths ?? Enumerable.Empty<string>())
        {
            fromRootPath = Path.Combine(fromRootPath ?? string.Empty, path);
        }
        string serverPath = Path.Combine(_env.ContentRootPath, "Files", fromRootPath ?? string.Empty);

        if (!System.IO.Directory.Exists(serverPath))
            throw new Exception("Server Directory Not Exists");
        //    return BadRequest("Server directory Not Exists");


        return Path.Combine(_env.ContentRootPath, "Files", fromRootPath ?? string.Empty );
        
        //if (fromRootPath == null)
        //    return Path.Combine(_env.ContentRootPath, "Files");
        //else
        //    return Path.Combine(_env.ContentRootPath, "Files", fromRootPath);

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
    [HttpGet("test122")]
    public IActionResult QueryInFolder_bak(string? queryDirJoin, string? fileFilterJoin)
    {
        try
        {
            string[]? queryDirPath = queryDirJoin?.Split("?");
            string[]? fileFilter = fileFilterJoin?.Split("?");

            // 서버 파일관리 폴더 Root 
            string serverDirectory = Path.Combine(_env.ContentRootPath, "Files");

            // 조회할 폴더 파라미터가 공백일 경우 오류
            foreach (var item in queryDirPath ?? Enumerable.Empty<string>())
            {
                if (string.IsNullOrEmpty(item))
                    return BadRequest("Server Directory is empty");
            }
            // 조회할 파일 필터가 파라미터가 공백일 경우 오류
            foreach (var item in fileFilter ?? Enumerable.Empty<string>())
            {
                if (string.IsNullOrEmpty(item))
                    return BadRequest("Search pattern is empty");
            }

            // 조회할 폴더 string :  서버 root폴더 + 파라미터
            foreach (var item in queryDirPath ?? Enumerable.Empty<string>())
            {
                if (string.IsNullOrEmpty(item)) continue;
                serverDirectory = Path.Combine(serverDirectory ?? string.Empty, item);
            }

            //서버경로가 없어면 오류
            if (serverDirectory == null)
                return BadRequest("Server Directory1 Not Exists");

            if (!Directory.Exists(serverDirectory))
                return BadRequest("Server Directory2 Not Exists");


            // file search 패턴이 있을 경우 한번에 처리할 방법이 없어서 패턴 별로 파일을 찾차서 List에 추가함
            List<string> filesPathList = new List<string>();
            if (fileFilter == null || fileFilter.Length <= 0)
            {
                filesPathList.AddRange(Directory.GetFiles(serverDirectory, "*.*"));
            }
            else
            {
                foreach (var item in fileFilter ?? Enumerable.Empty<string>())
                {
                    filesPathList.AddRange(Directory.GetFiles(serverDirectory, item));
                }
            }

            // 파일정보 생성: 파일 경로에서 파일명 및 파일 크기 조회
            var fileList = filesPathList.Select(s => new
            {
                fileName = Path.GetFileName(s),
                fileSize = this.SizeConverter(new FileInfo(s).Length)
            });


            // 해당 폴더의 폴더 목록
            var dirs = Directory.GetDirectories(serverDirectory);
            var dirList = dirs.Select(s => new
            {
                dirName = Path.GetFileName(s),
                updated = new FileInfo(s).LastWriteTime
            });

            // 반환 값: 조회 한 서버 폴더, 파일목록, 폴더목록
            return Ok(new { queryDirPath, fileList, dirList });

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("GetPdf")]
    public async Task<IActionResult> GetPdfAsync([Required] string fileName, string? subDirectory)
    {
        try
        {
            var serverPath = GetDirPathString(subDirectory);

            string filePath = Path.Combine(serverPath, fileName);

            if (!System.IO.File.Exists(filePath))
                return BadRequest();

            var ms = await _fileService.DownloadFileAsync(fileName, serverPath);
            //return new FileStreamResult(ms, "application/pdf");


            return new FileStreamResult(ms, GetContentType(fileName));
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: ex.HResult.ToString());
            
        }


        //var bytes = System.IO.File.ReadAllBytes(filePath);
        //return new FileContentResult(bytes, "application/pdf");
    }
    [HttpGet("GetDoc2")]
    public async Task<IActionResult> GetDoc2Async([Required] string fileName, string? subDirectory)
    {
        try
        {
            var serverPath = GetDirPathString(subDirectory);

            string filePath = Path.Combine(serverPath, fileName);

            if (!System.IO.File.Exists(filePath))
                return BadRequest();

            //var ms = await _fileService.DownloadFileAsync(fileName, serverPath);

            //return new FileStreamResult(ms, GetContentType(fileName))  ;

            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, GetContentType(fileName), true);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: ex.HResult.ToString());
            
        }


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