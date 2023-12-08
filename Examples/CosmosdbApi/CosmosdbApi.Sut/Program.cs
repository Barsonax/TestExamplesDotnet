using System.Net.Security;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

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

var app = builder.Build();

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<CosmosClient>();
var databaseResponse = await context.CreateDatabaseIfNotExistsAsync("CosmosdbApi");
var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties
{
    Id = "Blogs",
    PartitionKeyPath = "/Url"
});

app.MapGet("blogs", (CosmosClient cosmosClient) =>
{
    var results = cosmosClient
        .GetDatabase("CosmosdbApi")
        .GetContainer("Blogs")
        .GetItemQueryIterator<Blog>()
        .GetAllAsync();

    return Task.FromResult(TypedResults.Json(results));
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
    public required string Id { get; set; }
    public required string Url { get; set; }
}

public static class FeedIteratorExtensions
{
    public static async IAsyncEnumerable<T> GetAllAsync<T>(this FeedIterator<T> iterator)
    {
        while (iterator.HasMoreResults)
            foreach (var item in await iterator.ReadNextAsync().ConfigureAwait(false))
                yield return item;
    }
}
