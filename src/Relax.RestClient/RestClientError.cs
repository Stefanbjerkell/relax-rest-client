namespace Relax.RestClient
{
    public class RestClientError
    {
        public RestClientError(string error, string? reason)
        {
            Message = error;
            Reason = reason;
        }

        public string Message { get; set; }

        public string? Reason { get; set; } 
    }
}