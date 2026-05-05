using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Đây là demo đơn giản, trong thực tế cần kiểm tra database
        if (request.Username == "admin" && request.Password == "password")
        {
            return Ok(new
            {
                Token = "demo-jwt-token-12345",
                Username = request.Username,
                ExpiresIn = 3600
            });
        }

        return Unauthorized(new { Message = "Invalid username or password" });
    }

    [HttpPost("validate")]
    public IActionResult ValidateToken([FromBody] TokenRequest request)
    {
        // Đây là demo đơn giản, trong thực tế cần validate JWT token
        if (!string.IsNullOrEmpty(request.Token))
        {
            return Ok(new
            {
                IsValid = true,
                Username = "admin"
            });
        }

        return BadRequest(new { Message = "Invalid token" });
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class TokenRequest
{
    public string Token { get; set; } = string.Empty;
}
