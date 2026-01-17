using Leoni.Dtos;

namespace Leoni.Services.Interfaces
{
    public interface IwebhookService
    {
        Task HandleEvent(
         string rawBody,
         string gitHubEvent,
         string signatureHeader);
    }
}

