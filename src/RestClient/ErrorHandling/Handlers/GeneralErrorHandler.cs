using System.Net;

namespace RestClient.ErrorHandling.Handlers
{
    public class GeneralErrorHandler : IRestClientErrorHandler
    {
        private HandleError? _handleError;
        private GeneralErrorHandlerActions _actions;

        public GeneralErrorHandler()
        {
            _actions = new GeneralErrorHandlerActions();
        }

        public GeneralErrorHandler Throws(Exception exception)
        {
            _actions.Exception = exception;
            return this;
        }

        public GeneralErrorHandler SetBody(string body)
        {
            _actions.Content = body;
            return this;
        }

        public GeneralErrorHandler SetStatusCode(HttpStatusCode newStatus)
        {
            _actions.StatusCode = newStatus;
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

        public virtual async Task Handle(HttpResponseMessage httpResponse, RestClientResponse response)
        {
            if(_actions.Exception is not null)
            {
                throw _actions.Exception;
            }

            if (_handleError is not null)
            {
                await _handleError(httpResponse, response);
            }


        }
    }

    public class GeneralErrorHandlerActions
    {
        // Replace the content of the RestClientResult.
        public string? Content { get; set; }

        // Replace the status code of the RestClientResult
        public HttpStatusCode? StatusCode { get; set; }

        // Throws a custome exception.
        public Exception? Exception { get; set; }
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
            _request.ErrorHandlers.Add(_handler);
            return _request;
        }

    }

    public delegate Task HandleError(HttpResponseMessage response, RestClientResponse result);

    public static class GeneralErrorHandlerExtensions
    {
        public static GeneralErrorHandlerBuilder OnError(this RestClientRequest request)
        {
            var handler = new GeneralErrorHandler();

            return new GeneralErrorHandlerBuilder(handler, request);
        }
    }
}
