using RestClient.Validation;

namespace RestClient
{
    public class RestClientSettings
    {
        [Uri(ErrorMessage = "RestClientSettings.BaseUrl must be a valid uri.")]
        public string BaseUrl { get; set; } = "";

        public int TimeoutInSeconds { get; set; } = 30;

        public string? UserAgent { get; set;}
    }
}