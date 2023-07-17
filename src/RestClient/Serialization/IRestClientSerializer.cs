namespace RestClient.Serialization
{
    public interface IRestClientSerializer
    {
        public T? Deserialize<T>(string json);

        public string? Serialize(object item);
    }
}
