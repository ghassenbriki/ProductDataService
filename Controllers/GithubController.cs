using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leoni.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        [HttpPost("webhook")]
        public async Task<ActionResult> Webhook()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            return Ok();
        }

    }
}
