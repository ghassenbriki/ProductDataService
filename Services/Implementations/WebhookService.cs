using Leoni.Dtos;
using Leoni.Services.Interfaces;
using Leoni.Utils;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Leoni.Services.Implementations
{
    public class WebhookService : IwebhookService
    {
        private readonly GitHubOptions _options;


        public WebhookService(IOptions<GitHubOptions> opts)
        {
            _options = opts.Value;   // binding
        }
        public async Task HandleEvent(WebhookDto dto, string payload, string githubEvt, string signatureHeader)
        {

            var secret = _options.WebhookSecret;
            var isValidSecret = SecurityConfig.IsValidGitHubSignature(payload, signatureHeader, secret);



            if (!isValidSecret)
                throw new UnauthorizedAccessException("Invalid GitHub signature");

            switch (githubEvt)
            {
                case "push":
                    await HandlePushAsync(dto);
                    break;

                default:
                    break;
            }

        }



        private Task HandlePushAsync(WebhookDto dto)
        {
            var repo = dto.Repository.FullName;
            var branch = dto.Ref;

            Console.WriteLine($"Push on {branch} in {repo}");

            return Task.CompletedTask;
        }

    }
}
