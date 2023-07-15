using Moq.Protected;
using Moq;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using System.Linq.Expressions;

namespace RestClient.Moq
{
    public class MockHttpClientBuilder
    {
        private const string BASE_URL = "http://api.com";

        private Mock<HttpMessageHandler> _mockHandler;
        private JsonSerializerOptions _jsonOptions;
        public MockHttpClientBuilder(JsonSerializerOptions? jsonOptions = null) 
        {
            _mockHandler = new Mock<HttpMessageHandler>();
            _jsonOptions = jsonOptions ?? new JsonSerializerOptions();
        }
        public MockHttpClientBuilder WithDefaultResponse(HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            var response = CreateResponseMessage(status,body);
            var requestMessage = ItExpr.IsAny<HttpRequestMessage>();

            SetUpMock(requestMessage, response, delayInMilliseconds);
            return this;
        }

        public MockHttpClientBuilder Operation(HttpMethod method, string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            var response = CreateResponseMessage(status, body);
            var requestMessage = ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.AbsolutePath.Equals($"/{path}") && x.Method == method);

            SetUpMock(requestMessage, response, delayInMilliseconds);
            return this;
        }

        public MockHttpClientBuilder Get(string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            return Operation(HttpMethod.Get, path, status, body, delayInMilliseconds);
        }

        public MockHttpClientBuilder Post(string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            return Operation(HttpMethod.Post, path, status, body, delayInMilliseconds);
        }

        public MockHttpClientBuilder Put(string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
        {
            return Operation(HttpMethod.Put, path, status, body, delayInMilliseconds);
        }

        public MockHttpClientBuilder Delete(string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
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

        private void SetUpMock(Expression expression, HttpResponseMessage response, int milliseconds)
        {
            var cancelationToken = ItExpr.IsAny<CancellationToken>();

            _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", expression, cancelationToken)
            .Callback(() => Thread.Sleep(milliseconds))
            .ReturnsAsync((HttpRequestMessage message, CancellationToken token) =>
            {
                response.RequestMessage = message;
                return response;
            });
        }

        public HttpClient Build()
        {
            var client = new HttpClient(_mockHandler.Object)
            {
                BaseAddress = new Uri(BASE_URL)
            };
            return client;
        }
    }
}