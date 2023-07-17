using System.Text.Json;

namespace RestClient.Serializers;

public static class DefaultSerializerExtensions
{
    public static RestClientOptionsBuilder AddDefaultSerializer(this RestClientOptionsBuilder builder, JsonSerializerOptions options)
    {
        HttpRestClient.DefaultSerializer = new DefaultSerializer(options);
        return builder;

    }
}