using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using OpenAI_API;

namespace MyWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OpenAIController : ControllerBase
{
    [HttpPost("GetAnswer")]
    public IActionResult GetAnswer([FromBody] string prompt)
    {
        //your OpenAI API key
        string apiKey = "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";
        string answer = string.Empty;
        var openai = new OpenAIAPI(apiKey);
        CompletionRequest completion = new CompletionRequest();
        completion.Prompt = prompt;
        //completion.Model = OpenAI_API.Model.DavinciText;
        completion.Model = OpenAI_API.Models.Model.DavinciText;
        completion.MaxTokens = 4000;
        var result = openai.Completions.CreateCompletionAsync(completion);
        if (result != null)
        {
            foreach (var item in result.Result.Completions)
            {
                answer = item.Text;
            }
            return Ok(answer);
        }
        else
        {
            return BadRequest("Not found");
        }
    }

}
