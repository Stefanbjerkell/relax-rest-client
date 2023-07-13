using System.Net;
using System.Text.Json;
using Relax.RestClient.ErrorHandling;

namespace Relax.RestClient
{
    public class RestClientResponse<T> where T : class
    {
        public RestClientResponse(HttpResponseMessage response, RestClientRequest request,string stringContent, JsonSerializerOptions jsonOptions) {
        
            Data = JsonSerializer.Deserialize<T>(stringContent, jsonOptions);
            StringContent = stringContent;
            ResponseMessage = response;
            StatusCode = response.StatusCode;
            Request = request;
            IsSuccessfull = true;
        }

        public RestClientResponse(RestClientError error, HttpResponseMessage response, RestClientRequest request)
        {
            Request = request; 
            ResponseMessage = response;
            StringContent = error.Message;
            StatusCode = response.StatusCode;
            IsSuccessfull = false;
            Error = error;
        }

        public RestClientResponse(RestClientErrorHandlerResult handlerResult, HttpResponseMessage response, RestClientError error, RestClientRequest request, JsonSerializerOptions jsonOptions)
        {
            if(handlerResult.Exception != null) { throw handlerResult.Exception; }

            StatusCode = handlerResult.StatusCode ?? response.StatusCode;
            StringContent = handlerResult.Content ?? error.Message;
            Error = error;
            Data = string.IsNullOrEmpty(handlerResult.Content) ? null : JsonSerializer.Deserialize<T>(handlerResult.Content, jsonOptions);
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
    }
}