using System.Text.Json.Serialization;

namespace Leoni.Dtos
{
    public class WebhookDto
    {
        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        [JsonPropertyName("repository")]
        public Repository Repository { get; set; }
    }

    public class Repository
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("owner")]
        public Owner Owner { get; set; }
    }

    public class Owner
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }
}