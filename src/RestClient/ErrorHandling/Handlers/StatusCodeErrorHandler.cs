using System.Net;
using RestClient.ErrorHandling;
using RestClient.ErrorHandling.Handlers;

namespace RestClient.ErrorHandlers;

public class StatusCodeErrorHandler : GeneralErrorHandler , IRestClientErrorHandler
{
    private HttpStatusCode _statusCode;

    public StatusCodeErrorHandler(HttpStatusCode statusCode) 
    {
        _statusCode = statusCode;
    }

    public override bool CanHandle(HttpResponseMessage httpResponse)
    {
        return httpResponse.StatusCode == _statusCode;
    }

}

public static class StatusCodeErrorHandlerExtensions
{
    public static GeneralErrorHandlerBuilder OnError(this RestClientRequest request, HttpStatusCode statusCode)
    {
        var handler = new StatusCodeErrorHandler(statusCode);

        return new GeneralErrorHandlerBuilder(handler, request);
    }
}
