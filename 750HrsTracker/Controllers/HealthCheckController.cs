using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _750HrsTracker.Controllers
{
    [Route("health-check")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> HealthCheckAsync()
        {

            return Ok(new HealthCheckResponse { Response = "I'm alive :)" });
        }
    }

    public class HealthCheckResponse
    {
        public string Response { get; set; }
    }
}
