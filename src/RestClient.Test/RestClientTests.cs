namespace RestClient.Test;

public class RestClientTests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public async Task Success()
    {
        var body = "success!";
        var path = "good-path";

        var httpClient = HttpMock.SetupClient()
            .Get(path, HttpStatusCode.OK, body)
            .Build();

        var restClient = new RestClient(httpClient);

        var result = await restClient.Get(path).Execute();

        var data = result.Content<string>();

        Assert.That(result.IsSuccessfull, Is.True);
        Assert.That(data, Is.EqualTo(body));
    }

    [Test]
    public async Task ErrorHandled()
    {
        var body = new TestError();
        var path = "bad-path";

        var httpClient = HttpMock.SetupClient()
            .Get(path, HttpStatusCode.BadRequest, body)
            .Build();

        var restClient = new RestClient(httpClient);

        var result = await restClient.Get(path)
            .OnError(HttpStatusCode.BadRequest).Do(async (response, result) => 
            { 
                result.StatusCode = HttpStatusCode.OK; 
            })
            .Execute();

        Assert.That(result.IsSuccessfull, Is.True);
        Assert.That(result.Error,Is.Not.Null);
        Assert.That(result.Error.StatusCode , Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(result.Error.Handled, Is.EqualTo(true));
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }


    public class TestError
    {
        public string ErrorMessage { get; set; } = "Something went wrong!";
    }
}