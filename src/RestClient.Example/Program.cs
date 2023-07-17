using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RestClient;
using RestClient.Serializers.Newtonsoft;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHttpRestClient()
            .AddNewtonsoftDefaultSerializer(new JsonSerializerSettings());
    })
    .Build();

// TODO! Add examples.
// More examples will come here soon.

var client = new HttpRestClient("https://localhost:4200");

Console.WriteLine("Client Serializer: " + client.Serializer?.GetType().Name);
Console.WriteLine("Default Serializer: " + HttpRestClient.DefaultSerializer.GetType().Name);

Console.ReadLine();
