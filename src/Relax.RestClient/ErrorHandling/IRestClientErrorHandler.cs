namespace Relax.RestClient.ErrorHandling
{
    public interface IRestClientErrorHandler
    {
        bool CanHandle(HttpResponseMessage httpResponse);

        Task<RestClientErrorHandlerResult> Handle(HttpResponseMessage httpResponse);
    }


}

