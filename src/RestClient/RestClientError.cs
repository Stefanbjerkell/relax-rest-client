using System.Net;

namespace RestClient
{
    public class RestClientError
    {
        public RestClientError(HttpStatusCode status, string error, string? reason)
        {
            StatusCode = status;
            Message = error;
            Reason = reason;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public string? Reason { get; set; }

        public bool Handled { get; set; }
    }
}