using System.Net.Security;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CosmosClient>(_ =>
{
    CosmosClientOptions cosmosClientOptions = new()
    {
        SerializerOptions = new CosmosSerializationOptions
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        },
        ConnectionMode = ConnectionMode.Gateway,
        RequestTimeout = TimeSpan.FromSeconds(5),
        HttpClientFactory = () => new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            SslOptions = new SslClientAuthenticationOptions
            {
                // Leave certs unvalidated for debugging
                RemoteCertificateValidationCallback = delegate { return true; },
            },
        }, disposeHandler: false)
    };

    return new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        cosmosClientOptions);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<CosmosClient>();
    var databaseResponse = await context.CreateDatabaseIfNotExistsAsync("CosmosdbApi");
    var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties()
    {
        Id = "Blogs",
        PartitionKeyPath = "/Url"
    });
    await containerResponse.Container.UpsertItemAsync(new Blog
    {
        Id = "1",
        Url = "https://blog.photogrammer.net/"
    });
}

app.MapGet("blogs", async (CosmosClient cosmosClient) =>
{
    var result = await cosmosClient
        .GetDatabase("CosmosdbApi")
        .GetContainer("Blogs")
        .ReadManyItemsAsync<Blog>(new (string id, PartitionKey partitionKey)[] { new("1", PartitionKey.None) });

    return TypedResults.Json(result);
});

await app.RunAsync();

namespace Api.MsSql.Sut
{
    public partial class Program
    {
    }
}

public class Blog
{
    public string Id { get; set; }
    public required string Url { get; set; }
}
