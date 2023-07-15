namespace RestClient.ErrorHandling
{
    public interface IRestClientErrorHandler
    {
        bool CanHandle(HttpResponseMessage httpResponse);

        Task Handle(HttpResponseMessage httpResponse, RestClientResponse response);
    }


}

