using Newtonsoft.Json;
using RestClient.Serialization;

namespace RestClient.Serializers.Newtonsoft
{
    public class NewtonsoftSerializer : IRestClientSerializer
    {
        public JsonSerializerSettings Settings { get; set; }

        public NewtonsoftSerializer(JsonSerializerSettings? settings = null)
        {
            Settings = settings ?? new JsonSerializerSettings();
        }

        public T? Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string? Serialize(object item)
        {
            return JsonConvert.SerializeObject(item);
        }
    }
}