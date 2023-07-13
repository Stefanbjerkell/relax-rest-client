using System.Net;
using System.Text.Json;
using Relax.RestClient.ErrorHandling;

namespace Relax.RestClient
{
    public class RestClientResponse
    {
        private JsonSerializerOptions _jsonOptions;

        public RestClientResponse(HttpResponseMessage response, RestClientRequest request,string stringContent, JsonSerializerOptions jsonOptions)
        { 
            StringContent = stringContent;
            ResponseMessage = response;
            StatusCode = response.StatusCode;
            Request = request;

            _jsonOptions = jsonOptions ?? new JsonSerializerOptions();
        }

        public RestClientResponse(RestClientError error, HttpResponseMessage response, RestClientRequest request, JsonSerializerOptions jsonOptions)
        {
            Request = request; 
            ResponseMessage = response;
            StringContent = error.Message;
            StatusCode = response.StatusCode;
            Error = error;

            _jsonOptions = jsonOptions ?? new JsonSerializerOptions();
        }

        public HttpStatusCode StatusCode { get; set; }

        public string StringContent { get; set; }

        public HttpResponseMessage ResponseMessage { get; set; }

        public RestClientRequest Request { get; set; }

        public bool IsSuccessfull { get => (int)StatusCode < 400; }

        public RestClientError? Error { get; set; }

        public T? Content<T>() where T : class
        {
            return JsonSerializer.Deserialize<T>(StringContent, _jsonOptions);
        }
    }
}