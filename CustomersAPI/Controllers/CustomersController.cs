using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomersAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    // Protected endpoint - requires JWT token
    [Authorize]
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "Catcher Wong", "James Li" };
    }

    // Protected endpoint - requires JWT token
    [Authorize]
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return $"Catcher Wong - {id}";
    }

    // Unprotected endpoint - for comparison
    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult GetPublic()
    {
        return Ok(new
        {
            message = "This is a public endpoint, no authentication required!",
            data = new string[] { "Public Data 1", "Public Data 2" }
        });
    }

    // Debug endpoint to check configuration
    [AllowAnonymous]
    [HttpGet("debug")]
    public IActionResult GetDebug()
    {
        var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var audienceSection = config.GetSection("Audience");
        
        return Ok(new
        {
            message = "Debug info",
            secret_length = audienceSection["Secret"]?.Length ?? 0,
            issuer = audienceSection["Iss"],
            audience = audienceSection["Aud"],
            has_auth_header = Request.Headers.ContainsKey("Authorization"),
            auth_header = Request.Headers["Authorization"].ToString()
        });
    }
}
