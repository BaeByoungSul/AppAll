using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Database;
using Services.DbService;
using System.Data;

namespace MyWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DbController : ControllerBase
{
    private readonly IDbService _dbService;
    public DbController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost("ExecNonQuery")]
    public IActionResult ExecNonQuery([FromBody] List<MyCommand> cmds)
    {
        try
        {
            // PrintCommand(cmds);
            List<DBOutPut> outputList = _dbService.ExecNonQuery(cmds);
            return Ok( new 
            { 
                ReturnCode = "S",
                OutputList = outputList 
            });
        }
        catch (InvalidDbException ex)
        {
            return BadRequest( ex.Message );
        }
        catch (Exception ex)
        {
            //if (ex.Source)
            return Problem(detail: ex.Message, title: ex.HResult.ToString());

        }

    }

    [HttpPost("ExecNonQueryAsync")]
    public async Task<IActionResult> ExecNonQueryAsync([FromBody] List<MyCommand> cmds)
    {
        try
        {
            //PrintCommand(cmds);
            List<DBOutPut> outputList = await _dbService.ExecNonQueryAsync(cmds);
            return Ok(new
            {
                ReturnCode = "S",
                OutputList = outputList
            });

        }
        catch (InvalidDbException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            //if (ex.Source)
            return Problem(detail: ex.Message, title: ex.HResult.ToString());

        }

    }

    [HttpPost("GetDataSet")]
    public IActionResult GetDataSet(MyCommand cmd)
    {
        try
        {
            DataSet ds = _dbService.GetDataSet(cmd);
            return Ok(ds);
        }
        catch (InvalidDbException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: ex.HResult.ToString());
            //                return BadRequest(ex.Message);
        }

    }
    
    [HttpPost("GetDataSetAsync")]
    public async Task<IActionResult> GetDataSetAsync(MyCommand cmd)
    {
        try
        {
            DataSet ds = await _dbService.GetDataSetAsync(cmd);
            return Ok(ds);
        }
        catch (InvalidDbException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: ex.HResult.ToString());
            //                return BadRequest(ex.Message);
        }

    }
    private void PrintCommand ( List<MyCommand> cmds)
    {
        // print paramete command loop
        foreach (var command in cmds)
        {
            Console.WriteLine($"{command.CommandName}");
            Console.WriteLine($"{command.ConnectionName}");
            Console.WriteLine($"{command.CommandType}");
            Console.WriteLine($"{command.CommandText}");
            foreach (var para in command.Parameters ?? Enumerable.Empty<MyPara>())
            {
                Console.WriteLine($"{para.ParameterName}");
            }

            foreach (var pairs in command.ParaValues ?? Enumerable.Empty<Dictionary<string, string>>())
            {
                foreach (var item in pairs)
                {
                    Console.WriteLine($"{item.Key}: {item.Value} ");
                }
            }

        }

    }

}
