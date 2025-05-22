using Microsoft.AspNetCore.Mvc;

namespace MinhaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Olá do controller!");
        }
    }
}
