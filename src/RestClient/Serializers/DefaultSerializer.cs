using System.Text.Json;

namespace RestClient.Serializers
{
    public class DefaultSerializer : IRestClientSerializer
    {
        public JsonSerializerOptions Options { get; set; }

        public DefaultSerializer(JsonSerializerOptions? options = null)
        {
            Options = options ?? new JsonSerializerOptions();
        }

        public T? Deserialize<T>(string? json)
        {
            if (json == null) return default;

            return JsonSerializer.Deserialize<T>(json, Options);
        }

        public string? Serialize(object? item)
        {
            return JsonSerializer.Serialize(item, Options);
        }
    }
}
