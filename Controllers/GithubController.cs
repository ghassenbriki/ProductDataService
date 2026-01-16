using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leoni.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        [HttpPost("webhook")]
        public IActionResult Webhook()
        {
            return Ok();
        }

    }
}
