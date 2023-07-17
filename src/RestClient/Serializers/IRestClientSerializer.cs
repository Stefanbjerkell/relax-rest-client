namespace RestClient.Serializers
{
    public interface IRestClientSerializer
    {
        public T? Deserialize<T>(string? json);

        public string? Serialize(object? item);
    }
}
