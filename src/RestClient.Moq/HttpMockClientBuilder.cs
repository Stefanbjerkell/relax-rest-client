using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace RestClient.Mock
{
    public class HttpMockClientBuilder
    {
        private HttpMock _mockHandler;
        private JsonSerializerOptions _jsonOptions;
        public HttpMockClientBuilder(JsonSerializerOptions? jsonOptions = null) 
        {
            _mockHandler = new HttpMock();
            _jsonOptions = jsonOptions ?? new JsonSerializerOptions();
        }
        public HttpMockClientBuilder WithDefaultResponse(HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            var response = CreateResponseMessage(status,body);
            _mockHandler.DefaultResponse(response, delayInMilliseconds);

            return this;
        }

        public HttpMockClientBuilder Operation(HttpMethod method, string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            var response = CreateResponseMessage(status, body);
            _mockHandler.MockPath(method, path, response);
            return this;
        }

        public HttpMockClientBuilder Get(string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            return Operation(HttpMethod.Get, path, status, body, delayInMilliseconds);
        }

        public HttpMockClientBuilder Post(string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            return Operation(HttpMethod.Post, path, status, body, delayInMilliseconds);
        }

        public HttpMockClientBuilder Put(string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            return Operation(HttpMethod.Put, path, status, body, delayInMilliseconds);
        }

        public HttpMockClientBuilder Delete(string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            return Operation(HttpMethod.Delete, path, status, body, delayInMilliseconds);
        }



        private HttpResponseMessage CreateResponseMessage(HttpStatusCode status, object? body = null)
        {
            var response = new HttpResponseMessage()
            {
                Content = body is not null ? new StringContent(JsonSerializer.Serialize(body, _jsonOptions)) : null,
                StatusCode = status
            };

            if (response.Content is not null)
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            }

            return response;
        }

        public HttpClient Build()
        {
            return _mockHandler.GetClient();
        }
    }
}