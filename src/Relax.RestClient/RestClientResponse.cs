using System.Net;
using System.Text.Json;

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

        public RestClientResponse(RestClientErrorHandlerResult handlerResult, HttpResponseMessage response, string stringConent, RestClientRequest request)
        {
            StatusCode = handlerResult.StatusCode ?? response.StatusCode;
            Data = JsonSerializer.Deserialize<T>(handlerResult.Content ?? stringConent);
            Request = request; 
            ResponseMessage = response;
        }

        public HttpStatusCode StatusCode { get; set; }

        public T? Data { get; set; }

        public HttpResponseMessage ResponseMessage { get; set; }

        public RestClientRequest Request { get; set; }

        public bool IsSuccessfull { get; set; }

        public RestClientError? Error { get; set; }
    }
}