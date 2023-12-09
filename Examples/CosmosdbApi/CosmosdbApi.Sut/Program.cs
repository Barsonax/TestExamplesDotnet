using System.Net.Security;
using CosmosdbApi.Sut;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CosmosClient>(_ =>
{
    CosmosClientOptions cosmosClientOptions = new()
    {
        SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase },
        ConnectionMode = ConnectionMode.Gateway,
        RequestTimeout = TimeSpan.FromSeconds(5),
        HttpClientFactory = () => new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            SslOptions = new SslClientAuthenticationOptions
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true,
            },
        }, disposeHandler: false)
    };

    return new CosmosClient(builder.Configuration["DbConnectionString"], cosmosClientOptions);
});
builder.Services.AddSingleton<Database>(services => services.GetRequiredService<CosmosClient>().GetDatabase(builder.Configuration["Database"]));
builder.Services.AddTransient<CosmosDbInitializer>();

var app = builder.Build();
await app.Services.GetRequiredService<CosmosDbInitializer>().InitializeAsync();

app.MapGet("blogs", (Database database) =>
{
    var results = database
        .GetContainer("Blogs")
        .GetItemQueryIterator<Blog>()
        .GetAllAsync();

    return TypedResults.Json(results);
});

await app.RunAsync();

namespace Api.MsSql.Sut
{
    public partial class Program
    {
    }
}
