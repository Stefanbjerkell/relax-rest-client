using Microsoft.Extensions.DependencyInjection;

namespace RestClient
{
    public static class RestClientServiceExtensions
    {
        public static RestClientOptionsBuilder AddHttpRestClient(this IServiceCollection services)
        {
            return new RestClientOptionsBuilder();
        }
    }

    public class RestClientOptionsBuilder
    {

    }

}
