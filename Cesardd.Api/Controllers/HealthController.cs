using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Cesardd.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("HealthPolicy")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Response.Headers["Cache-Control"] = "public, max-age=60";
            return Ok();
        }
    }
}
