using System.Net;
using System.Text.Json;

namespace RestClient.Mock;

public class HttpMock : HttpMessageHandler
{
    private const string BASE_URL = "http://api.com";
    private List<MockedPath> _mockedPaths = new List<MockedPath>();

    public static HttpMockClientBuilder SetupClient(JsonSerializerOptions? jsonOptions = null)
    {
        return new HttpMockClientBuilder(jsonOptions);
    }

    public HttpClient GetClient()
    {
        return new HttpClient(this)
        {
            BaseAddress = new Uri(BASE_URL)
        };
    }

    /// <summary>
    /// Overriden SendAsync. This will look for a mocked reponse based on the incoming path.
    /// </summary>
    /// <param name="request">http request message</param>
    /// <param name="cancellationToken">Cancelation token.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">Thrown when no mock response is found.</exception>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var match = _mockedPaths.Where(x =>
            (x.Path == "*" || IsPathMatch(request, x.Path)) &&
            (x.Method == null || x.Method == request.Method));

        var bestMatch =
            (match.FirstOrDefault(x => x.Method == request.Method && IsPathMatch(request, x.Path)) ??
            match.FirstOrDefault(x => x.Method == request.Method && x.Path == "*") ??
            match.FirstOrDefault(x => x.Method == null && IsPathMatch(request, x.Path)) ??
            match.FirstOrDefault(x => x.Method == null && x.Path == "*"))
            ?? throw new HttpRequestException("Mock not set up for this request!");
        
        await Task.Delay(bestMatch.MillisecondDelay, cancellationToken);
        
        return bestMatch.Response;
    }

    /// <summary>
    /// Overriden Send. This will look for a mocked reponse based on the incoming path.
    /// </summary>
    /// <param name="request">http request message</param>
    /// <param name="cancellationToken">Cancelation token.</param>
    /// <returns></returns>
    /// <exception cref="HttpRequestException">Thrown when no mock response is found.</exception>
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var match = _mockedPaths.Where(x =>
           (x.Path == "*" || IsPathMatch(request, x.Path)) &&
           (x.Method == null || x.Method == request.Method));

        var bestMatch =
            (match.FirstOrDefault(x => x.Method == request.Method && IsPathMatch(request, x.Path)) ??
            match.FirstOrDefault(x => x.Method == request.Method && x.Path == "*") ??
            match.FirstOrDefault(x => x.Method == null && IsPathMatch(request, x.Path)) ??
            match.FirstOrDefault(x => x.Method == null && x.Path == "*"))
            ?? throw new HttpRequestException("Mock not set up for this request!");

        Thread.Sleep(bestMatch.MillisecondDelay);

        return bestMatch.Response;
    }

    private static bool IsPathMatch(HttpRequestMessage request,  string path)
    {
        return request.RequestUri!.AbsolutePath.Equals($"/{path}", StringComparison.CurrentCultureIgnoreCase);
    }

    public void DefaultResponse(HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
    {
        var mock = new MockedPath()
        {
            MillisecondDelay = delayInMilliseconds,
            Response = HttpMockClientHelpers.CreateResponseMessage(status, body)
        };

        _mockedPaths.Add(mock);
    }

    public void DefaultResponse(HttpResponseMessage response, int delayInMilliseconds = 0)
    {
        var mock = new MockedPath()
        {
            Response = response,
            MillisecondDelay = delayInMilliseconds
        };

        _mockedPaths.Add(mock);

    }

    public void MockPath(string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
    {
        var mock = new MockedPath()
        {
            MillisecondDelay = delayInMilliseconds,
            Path = path,
            Response = HttpMockClientHelpers.CreateResponseMessage(status, body)
        };

        _mockedPaths.Add(mock);
    }

    public void MockPath(string path, HttpResponseMessage response, int delayInMilliseconds = 0)
    {
        var mock = new MockedPath()
        {
            MillisecondDelay = delayInMilliseconds,
            Path = path,
            Response = response
        };

        _mockedPaths.Add(mock);
    }

    public void MockPath(HttpMethod method, string path, HttpStatusCode status, object? body = null, int delayInMilliseconds = 0)
    {
        var mock = new MockedPath()
        {
            MillisecondDelay = delayInMilliseconds,
            Method = method,
            Path = path,
            Response = HttpMockClientHelpers.CreateResponseMessage(status, body)
        };

        _mockedPaths.Add(mock);
    }

    public void MockPath(HttpMethod method, string path, HttpResponseMessage response, int delayInMilliseconds = 0)
    {
        var mock = new MockedPath()
        {
            MillisecondDelay = delayInMilliseconds,
            Method = method,
            Path = path,
            Response = response
        };

        _mockedPaths.Add(mock);
    }
}

internal class MockedPath
{
    public string Path { get; set; } = "*";

    public HttpMethod? Method { get; set; } = null;

    public HttpResponseMessage Response { get; set; } = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };

    public int MillisecondDelay { get; set; } = 0;

}