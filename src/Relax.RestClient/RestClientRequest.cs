using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Relax.RestClient.ErrorHandlers;
using Relax.RestClient.ErrorHandling;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Relax.RestClient
{
    public class RestClientRequest
    {
        private HttpMethod _method;
        private string _path;
        private HttpClient _client;
        private JsonSerializerOptions _jsonOptions;


        public Dictionary<string, string> QueryParameters { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public HttpContent? Content { get; internal set; }

        public List<IRestClientErrorHandler> ErrorHandlers { get; set; } = new List<IRestClientErrorHandler>();

        public string? StringBody { get; set; }

        public RestClientRequest(HttpMethod method, string path, HttpClient client, JsonSerializerOptions jsonOptions)
        {
            _method = method;
            _path = path;
            _client = client;
            _jsonOptions = jsonOptions;
        }

        // Url Parameters

        public RestClientRequest WithParameter(string key, string value)
        {
            _path = _path.Replace("{" + key + "}", value);
            return this;
        }

        public RestClientRequest WithParameter(object value, [CallerArgumentExpression("value")] string? name = null)
        {
            var paramValue = value.ToString();

            if (name is null || paramValue is null) throw new Exception("");

            return WithParameter(name, paramValue.ToString());
        }

        public RestClientRequest WithParameter<T>(string key, T value) where T : Enum
        {
            return WithParameter(key, value.ToString());
        }

        public RestClientRequest WithParameter(string key, bool value)
        {
            return WithParameter(key, value.ToString());
        }


        // Headers

        public RestClientRequest WithHeader(string name, string value)
        {
            Headers.Add(name, value);
            return this;
        }

        public RestClientRequest WithHeader(string name, object value)
        {
            var stringValue = value.ToString();

            if (!string.IsNullOrEmpty(stringValue))
            {
                return WithHeader(name, stringValue);
            }

            return this;
        }

        public RestClientRequest WithHeader(object value, [CallerArgumentExpression("value")] string? name = null)
        {
            var headerValue = value.ToString();

            if (name is null || headerValue is null) throw new Exception("Invalid header value!");

            return WithHeader(name, headerValue);
        }

        // Query Parameters

        public RestClientRequest WithQueryParameter(string name, string value)
        {
            QueryParameters.Add(name, value);
            return this;
        }

        public RestClientRequest WithQueryParameter(string name, object value)
        {
            var stringValue = value.ToString();

            if (!string.IsNullOrEmpty(stringValue))
            {
                return WithQueryParameter(name, stringValue);
            }

            return this;
        }

        public RestClientRequest WithQueryParameter(object value, [CallerArgumentExpression("value")] string? name = null)
        {
            var queryValue = value.ToString();

            if (name is null || queryValue is null) throw new Exception("Invalid query parameter value!");

            return WithQueryParameter(name, queryValue);
        }

        // Content

        public RestClientRequest WithJsonBody(object body)
        {
            StringBody = JsonSerializer.Serialize(body, _jsonOptions);
            var mediaType = "application/json";
            var content = new StringContent(StringBody, null, mediaType);

            return WithContent(content, mediaType);
        }

        public RestClientRequest WithContent(HttpContent content, string mediaType)
        {
            if (Content is not null)
            {
                throw new Exception("You can only set content once!");
            }

            Content = content;
            Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return this;
        }

        // Execution

        public async Task<HttpResponseMessage> ExecuteWithHttpResponse()
        {
            var request = ToHttpRequest();
            var response = await _client.SendAsync(request);

            return response;
        }

        public async Task<RestClientResponse<T>> Execute<T>() where T : class
        {
            var response = await ExecuteWithHttpResponse();
            var stringContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return new RestClientResponse<T>(response, this, stringContent, _jsonOptions);
            }

            var error = new RestClientError(response.StatusCode, stringContent, response.ReasonPhrase);
            var errorHandler = ErrorHandlers.FirstOrDefault(x => x.CanHandle(response));

            if (errorHandler != null)
            {
                var handlerResult = await errorHandler.Handle(response);
                error.Handled = true;
                return new RestClientResponse<T>(handlerResult, response, error, this, _jsonOptions);
            }
            else
            {
                return new RestClientResponse<T>(error, response, this);
            }
        }

        public async Task<T?> ExecuteWithDataResponse<T>() where T : class
        {
            var response = await Execute<T>();
            if (response.IsSuccessfull)
            {
                return response.Data;
            }

            throw new Exception(response.Error!.Message);

        }

        // Error Handling

        public RestClientRequest WithErrorHandler(IRestClientErrorHandler errorHandler)
        {
            ErrorHandlers.Add(errorHandler);
            return this;
        }

        // Helpers

        private HttpRequestMessage ToHttpRequest()
        {
            var path = QueryParameters is object && QueryParameters.Any() ? _path + "?" + string.Join("&", QueryParameters.Select(x => x.Key + "=" + x.Value)) : _path;
            var request = new HttpRequestMessage(_method, path);

            if (Content is not null)
            {
                request.Content = Content;
            }

            if (Headers is not null)
            {
                foreach (var header in Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            return request;
        }
    }


}

