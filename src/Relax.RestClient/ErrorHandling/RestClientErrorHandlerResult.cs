using System.Net;

namespace Relax.RestClient.ErrorHandling
{
    public class RestClientErrorHandlerResult
    {
        // Replace the content of the RestClientResult.
        public string? Content { get; set; }

        // Replace the status code of the RestClientResult
        public HttpStatusCode? StatusCode { get; set; }

        // Throws a custome exception.
        public Exception? Exception { get; set; }
    }


}

