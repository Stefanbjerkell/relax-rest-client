using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RestClient;
using RestClient.Serializers;
using RestClient.Serializers.Newtonsoft;
using System.Text.Json;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHttpRestClient()
            .AddNewtonsoftDefaultSerializer(new JsonSerializerSettings());
    })
    .Build();