using System.Net;

namespace Relax.RestClient.ErrorHandlers
{
    public class StatusCodeErrorHandler : GeneralErrorHandler , IRestClientErrorHandler
    {
        private HttpStatusCode _statusCode;
        private HandleError? _handleError;
        private RestClientErrorHandlerResult _handlerResult;

        public StatusCodeErrorHandler(HttpStatusCode statusCode) 
        {
            _statusCode = statusCode;
            _handlerResult = new RestClientErrorHandlerResult();
        }

        // Interface

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
}
