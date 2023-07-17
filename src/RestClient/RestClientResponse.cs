using System.Net;
using RestClient.Serializers;

namespace RestClient
{
    public class RestClientResponse
    {
        private IRestClientSerializer _serializer;

        public RestClientResponse(HttpResponseMessage response, RestClientRequest request,string stringContent, IRestClientSerializer serializer)
        { 
            StringContent = stringContent;
            ResponseMessage = response;
            StatusCode = response.StatusCode;
            Request = request;

            _serializer = serializer;
        }

        public RestClientResponse(RestClientError error, HttpResponseMessage response, RestClientRequest request, IRestClientSerializer serializer)
        {
            Request = request; 
            ResponseMessage = response;
            StringContent = error.Message;
            StatusCode = response.StatusCode;
            Error = error;

            _serializer = serializer;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string StringContent { get; set; }

        public HttpResponseMessage ResponseMessage { get; set; }

        public RestClientRequest Request { get; set; }

        public bool IsSuccessfull { get => (int)StatusCode < 400; }

        public RestClientError? Error { get; set; }

        public T? Content<T>() where T : class
        {
            return _serializer.Deserialize<T>(StringContent);
        }
    }
}