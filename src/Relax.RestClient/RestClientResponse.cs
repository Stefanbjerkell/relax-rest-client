using System.Net;
using System.Text.Json;
using Relax.RestClient.ErrorHandlers;
using Relax.RestClient.ErrorHandling;

namespace Relax.RestClient
{
    public class RestClientResponse<T> where T : class
    {
        public RestClientResponse(HttpResponseMessage response, RestClientRequest request, T? data) {
        
            Data = data;
            ResponseMessage = response;
            StatusCode = response.StatusCode;
            Request = request;
            IsSuccessfull = true;
        }

        public RestClientResponse(RestClientError error, HttpResponseMessage response, RestClientRequest request)
        {
            Request = request; 
            ResponseMessage = response;
            StatusCode = response.StatusCode;
            IsSuccessfull = false;
            Error = error;
        }

        public RestClientResponse(RestClientErrorHandlerResult handlerResult, HttpResponseMessage response, RestClientError error, RestClientRequest request)
        {
            if(handlerResult.Exception != null) { throw handlerResult.Exception; }

            StatusCode = handlerResult.StatusCode ?? response.StatusCode;
            StringContent = handlerResult.Content ?? error.Message;
            Error = error;
            ErrorHandled = true;
            Data = string.IsNullOrEmpty(handlerResult.Content) ? null : JsonSerializer.Deserialize<T>(handlerResult.Content);
            Request = request; 
            ResponseMessage = response;
            IsSuccessfull = (int)StatusCode < 400;
        }

        public HttpStatusCode StatusCode { get; set; }

        public T? Data { get; set; }

        public string StringContent { get; set; }

        public HttpResponseMessage ResponseMessage { get; set; }

        public RestClientRequest Request { get; set; }

        public bool IsSuccessfull { get; set; }

        public RestClientError? Error { get; set; }

        public bool ErrorHandled { get; set; }
    }
}