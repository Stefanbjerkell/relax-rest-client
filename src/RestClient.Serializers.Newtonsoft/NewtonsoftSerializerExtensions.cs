using Newtonsoft.Json;

namespace RestClient.Serializers.Newtonsoft;

public static class NewtonsoftSerializerExtensions
{
    public static RestClientOptionsBuilder AddNewtonsoftDefaultSerializer(this RestClientOptionsBuilder builder, JsonSerializerSettings settings)
    {
        HttpRestClient.DefaultSerializer = new NewtonsoftSerializer(settings);
        return builder;

    }
}