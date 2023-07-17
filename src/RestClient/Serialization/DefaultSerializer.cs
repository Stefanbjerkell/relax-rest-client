using System.Text.Json;

namespace RestClient.Serialization
{
    public class DefaultSerializer : IRestClientSerializer
    {
        public JsonSerializerOptions Options { get; set; }

        public DefaultSerializer(JsonSerializerOptions? options = null)
        {
            Options = options ?? new JsonSerializerOptions();
        }

        public T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, Options);
        }

        public string? Serialize(object item)
        {
            if (item == null) return null;

            return JsonSerializer.Serialize(item, Options);
        }
    }
}
