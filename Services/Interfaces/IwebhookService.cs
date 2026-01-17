using Leoni.Dtos;

namespace Leoni.Services.Interfaces
{
    public interface IwebhookService
    {
        Task HandleEvent(
         WebhookDto dto,
         string rawBody,
         string gitHubEvent,
         string signatureHeader);
    }
}

