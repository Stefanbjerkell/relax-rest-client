using System.Net;

namespace Relax.RestClient
{
    public class RestClientResponse<T> where T : class
    {
        public RestClientResponse(HttpResponseMessage response, RestClientRequest request, T? data) {
        
            Data = data;
            Response = response;
            Request = request;
            IsSuccessfull = true;
        }

        public RestClientResponse(RestClientError error, HttpResponseMessage response, RestClientRequest request)
        {
            Request = request; 
            Response = response;
            IsSuccessfull = false;
            Error = error;
        }

        public T? Data { get; set; }

        public HttpResponseMessage Response { get; set; }

        public RestClientRequest Request { get; set; }

        public bool IsSuccessfull { get; set; }

        public RestClientError? Error { get; set; }
    }
}