using RestClient.Serializers;
using System.Text;

namespace RestClient
{
    public class HttpRestClient
    {
        public static IRestClientSerializer DefaultSerializer { get; set; } = new DefaultSerializer();

        private readonly HttpClient _client;

        public IRestClientSerializer? Serializer { get; set; }

        public Dictionary<string, string> DefaultHeaders { get; set; } = new Dictionary<string, string>();  


        public HttpRestClient(string baseUrl, IRestClientSerializer? serializer = null)
        {
            Serializer = serializer;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
        }

        public HttpRestClient(RestClientSettings settings, IRestClientSerializer? serializer = null)
        {
            Serializer = serializer;
            _client = new HttpClient
            {
                BaseAddress = new Uri(settings.BaseUrl),
                Timeout = TimeSpan.FromSeconds(settings.TimeoutInSeconds)
            };

            if (!string.IsNullOrEmpty(settings.UserAgent))
            {
                _client.DefaultRequestHeaders.Add("User-Agent", settings.UserAgent);
            }

        }        

        public HttpRestClient(HttpMessageHandler messageHandler, IRestClientSerializer? serializer = null) 
            : this(new HttpClient(messageHandler), serializer)
        {

        }

        public HttpRestClient(HttpClient client, IRestClientSerializer? serializer = null)
        {
            Serializer = serializer;
            _client = client;
        }

        // Public

        public RestClientRequest Get(string path)
        {
            return new RestClientRequest(HttpMethod.Get, path, _client, Serializer ?? DefaultSerializer, DefaultHeaders);
        }

        public RestClientRequest Put(string path, object? body = null)
        {
            return new RestClientRequest(HttpMethod.Put, path, _client, Serializer ?? DefaultSerializer, DefaultHeaders)
                .WithJsonBody(body);
        }

        public RestClientRequest Post(string path, object? body = null)
        {
            return new RestClientRequest(HttpMethod.Post, path, _client, Serializer ?? DefaultSerializer, DefaultHeaders)
                .WithJsonBody(body);
        }

        public RestClientRequest Delete(string path)
        {
            return new RestClientRequest(HttpMethod.Delete, path, _client, Serializer ?? DefaultSerializer, DefaultHeaders);
        }

        // Options

        public void AddDefaultHeader(string key, string value)
        {
            if (DefaultHeaders.ContainsKey(key)) throw new Exception("Header with same name already added!");

            DefaultHeaders.Add(key, value);
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
}