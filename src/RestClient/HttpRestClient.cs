using System.Text;
using System.Text.Json;

namespace RestClient
{
    public class HttpRestClient
    {
        private HttpClient _client;

        public JsonSerializerOptions JsonOptions { get; set; }

        public Dictionary<string, string> DefaultHeaders { get; set; } = new Dictionary<string, string>();  


        public HttpRestClient(string baseUrl, JsonSerializerOptions? jsonOptions = null)
        {
            JsonOptions = jsonOptions ?? new JsonSerializerOptions();

            _client = new HttpClient();

            _client.BaseAddress = new Uri(baseUrl);
        }

        public HttpRestClient(RestClientSettings settings, JsonSerializerOptions? jsonOptions = null)
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

        public HttpRestClient(HttpMessageHandler messageHandler, JsonSerializerOptions? jsonOptions = null) 
            : this(new HttpClient(messageHandler), jsonOptions)
        {

        }

        public HttpRestClient(HttpClient client, JsonSerializerOptions? jsonOptions = null)
        {
            JsonOptions = jsonOptions ?? new JsonSerializerOptions();
            _client = client;
        }

        // Public

        public RestClientRequest Get(string path)
        {
            return new RestClientRequest(HttpMethod.Get, path, _client, JsonOptions, DefaultHeaders);
        }

        public RestClientRequest Put(string path, object? body = null)
        {
            return new RestClientRequest(HttpMethod.Put, path, _client, JsonOptions, DefaultHeaders)
                .WithJsonBody(body);
        }

        public RestClientRequest Post(string path, object? body = null)
        {
            return new RestClientRequest(HttpMethod.Post, path, _client, JsonOptions, DefaultHeaders)
                .WithJsonBody(body);
        }

        public RestClientRequest Delete(string path)
        {
            return new RestClientRequest(HttpMethod.Delete, path, _client, JsonOptions, DefaultHeaders);
        }

        // Options

        public void AddDefaultHeader(string key, string value)
        {
            if (DefaultHeaders.ContainsKey(key)) throw new Exception("Header with same name already added!");

            DefaultHeaders.Add(key, value);
        }

        public HttpRestClient AddJsonOptions(JsonSerializerOptions jsonOptions)
        {
            JsonOptions = jsonOptions;
            return this;
        }

        public HttpRestClient AddBasicAuth(string username, string password)
        {
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));

            AddDefaultHeader("Authorization", $"Basic {credentials}");

            return this;
        }

        public HttpRestClient AddAuthToken(string token, string schema = "Bearer")
        {
            AddDefaultHeader("Authorization", $"{schema} {token}");

            return this;
        }
    }

    public class RestClientSettings
    {
        public string BaseUrl { get; set; } = "";

        public int TimeoutInSeconds { get; set; } = 10;

        public string? UserAgent { get; set;}
    }
}