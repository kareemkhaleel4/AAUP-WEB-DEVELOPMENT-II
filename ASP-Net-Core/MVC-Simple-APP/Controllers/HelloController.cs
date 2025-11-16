using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
    [HttpGet("/{name}/greet")]
    public IActionResult greetName(string? name, [FromQuery] int? age)
    {
        if (string.IsNullOrEmpty(name))
        {
            return BadRequest("Name cannot be null or empty.");
        }   
        if (age.HasValue)
        {
            return Ok($"Hello {name}, you are {age} years old!");
        }
        return Ok($"Hello {name}!");
    }

    [HttpPost]
    public IActionResult test([FromBody]object data, [FromQuery]string param)
    {
        return Ok("Posted Data: from body " + data + " --- from Param " +param );
    }
}