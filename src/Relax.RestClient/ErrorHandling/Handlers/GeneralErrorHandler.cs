using System.Net;

namespace Relax.RestClient.ErrorHandling.Handlers
{
    public class GeneralErrorHandler : IRestClientErrorHandler
    {
        private HandleError? _handleError;
        private RestClientErrorHandlerResult _handlerResult;

        public GeneralErrorHandler()
        {
            _handlerResult = new RestClientErrorHandlerResult();
        }

        public GeneralErrorHandler Throws(Exception exception)
        {
            _handlerResult.Exception = exception;
            return this;
        }

        public GeneralErrorHandler SetBody(string body)
        {
            _handlerResult.Content = body;
            return this;
        }

        public GeneralErrorHandler SetStatusCode(HttpStatusCode newStatus)
        {
            _handlerResult.StatusCode = newStatus;
            return this;
        }

        public GeneralErrorHandler Do(HandleError handleError)
        {
            _handleError = handleError;
            return this;
        }

        // Interface

        public virtual bool CanHandle(HttpResponseMessage httpResponse)
        {
            return true;
        }

        public virtual async Task<RestClientErrorHandlerResult> Handle(HttpResponseMessage httpResponse)
        {
            if (_handleError is not null)
            {
                await _handleError(httpResponse, _handlerResult);
            }

            return await Task.FromResult(_handlerResult);
        }
    }

    public class GeneralErrorHandlerBuilder
    {
        private GeneralErrorHandler _handler;
        private RestClientRequest _request;

        public GeneralErrorHandlerBuilder(GeneralErrorHandler handler, RestClientRequest request)
        {
            _handler = handler;
            _request = request;
        }

        public RestClientRequest Throw(Exception ex)
        {
            _handler.Throws(ex);
            _request.ErrorHandlers.Add(_handler);
            return _request;
        }

        public RestClientRequest SetResponse(HttpStatusCode statusCode, string content)
        {
            _handler.SetStatusCode(statusCode);
            _handler.SetBody(content);
            _request.ErrorHandlers.Add(_handler);
            return _request;
        }

        public RestClientRequest Do(HandleError handleError)
        {
            _handler.Do(handleError);
            return _request;
        }

    }

    public delegate Task HandleError(HttpResponseMessage response, RestClientErrorHandlerResult result);

    public static class GeneralErrorHandlerExtensions
    {
        public static GeneralErrorHandlerBuilder OnError(this RestClientRequest request)
        {
            var handler = new GeneralErrorHandler();

            return new GeneralErrorHandlerBuilder(handler, request);
        }
    }
}
