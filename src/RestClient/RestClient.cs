using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using System.Runtime;
using System.Text.Json;

namespace RestClient
{
    public class RestClient
    {
        private HttpClient _client;

        public JsonSerializerOptions JsonOptions { get; set; }

        public RestClient(RestClientSettings settings, JsonSerializerOptions? jsonOptions = null)
        {
            JsonOptions = jsonOptions ?? new JsonSerializerOptions();

            _client = new HttpClient();

            _client.BaseAddress = new Uri(settings.BaseUrl);
            _client.Timeout = TimeSpan.FromSeconds(settings.TimeoutInSeconds);

            if (!string.IsNullOrEmpty(settings.UserAgent))
            {
                _client.DefaultRequestHeaders.Add("User-Agent", settings.UserAgent);
            }

        }        

        public RestClient(HttpMessageHandler messageHandler, JsonSerializerOptions? jsonOptions = null) 
            : this(new HttpClient(messageHandler), jsonOptions)
        {

        }

        public RestClient(HttpClient client, JsonSerializerOptions? jsonOptions = null)
        {
            JsonOptions = jsonOptions ?? new JsonSerializerOptions();
            _client = client;
        }

        // Public

        public RestClientRequest Get(string path)
        {
            return new RestClientRequest(HttpMethod.Get, path, _client, JsonOptions);
        }

        public RestClientRequest Put(string path)
        {
            return new RestClientRequest(HttpMethod.Put, path, _client, JsonOptions);
        }

        public RestClientRequest Post(string path)
        {
            return new RestClientRequest(HttpMethod.Post, path, _client, JsonOptions);
        }

        public RestClientRequest Delete(string path)
        {
            return new RestClientRequest(HttpMethod.Delete, path, _client, JsonOptions);
        }

        public RestClient WithJsonOptions(JsonSerializerOptions jsonOptions)
        {
            JsonOptions = jsonOptions;
            return this;
        }
    }

    public class RestClientSettings
    {
        public string BaseUrl { get; set; } = "";

        public int TimeoutInSeconds { get; set;}

        public string? UserAgent { get; set;}
    }
}