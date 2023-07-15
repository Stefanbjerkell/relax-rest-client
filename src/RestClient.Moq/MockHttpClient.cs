using Moq.Protected;
using Moq;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace RestClient.Moq
{
    public static class MockHttpClient
    {
        public static MockHttpClientBuilder Setup(JsonSerializerOptions? jsonOptions = null)
        {
            return new MockHttpClientBuilder(jsonOptions);
        }

        public static Mock<HttpMessageHandler> MockHttpClientResponse(HttpStatusCode code, object? response = null, string mediaType = "application/json", int delayMilliSeconds = 0, JsonSerializerOptions? jsonOptions = null)
        {
            jsonOptions ??= new JsonSerializerOptions();

            var mockResponse = new HttpResponseMessage()
            {
                Content = response is not null ? new StringContent(JsonSerializer.Serialize(response, jsonOptions)) : null,
                StatusCode = code
            };

            if (mockResponse.Content is not null)
            {
                mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            }
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback(() => Thread.Sleep(delayMilliSeconds))
            .ReturnsAsync((HttpRequestMessage message, CancellationToken token) =>
            {
                mockResponse.RequestMessage = message;
                return mockResponse;
            });

            return mockHandler;
        }
    }
}