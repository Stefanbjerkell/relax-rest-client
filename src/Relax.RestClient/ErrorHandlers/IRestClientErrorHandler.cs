namespace Relax.RestClient.ErrorHandlers
{
    public interface IRestClientErrorHandler
    {
        bool CanHandle(HttpResponseMessage httpResponse);

        Task<RestClientErrorHandlerResult> Handle(HttpResponseMessage httpResponse);
    }


}

