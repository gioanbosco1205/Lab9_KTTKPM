using Microsoft.AspNetCore.Mvc;

namespace CustomersAPI.Controllers;

[Route("api/[controller]")]
public class CustomersController : Controller
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "Catcher Wong", "James Li" };
    }

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return $"Catcher Wong - {id}";
    }
}
