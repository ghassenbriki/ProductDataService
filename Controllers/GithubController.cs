using Leoni.Dtos;
using Leoni.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Leoni.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class GithubController : ControllerBase
    {
        private readonly IwebhookService _webhookService;
        public GithubController(IwebhookService webhookService)
        {
            _webhookService = webhookService;
        }
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] WebhookDto dto)
        {
            try
            {
                Request.EnableBuffering();

                using var reader = new StreamReader(Request.Body, leaveOpen: true);
                var rawBody = await reader.ReadToEndAsync();
                Request.Body.Position = 0;

                var gitHubEvent = Request.Headers["X-GitHub-Event"].ToString();
                var signature = Request.Headers["X-Hub-Signature-256"].ToString();

                await _webhookService.HandleEvent(dto, rawBody, gitHubEvent, signature);

                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (JsonException ex)
            {
                return BadRequest("Invalid JSON payload "+ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
