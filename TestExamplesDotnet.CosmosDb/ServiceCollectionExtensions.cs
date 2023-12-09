using System.Net.Security;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Testcontainers.CosmosDb;

namespace TestExamplesDotnet.CosmosDb;

public static class ServiceCollectionExtensions
{
    public static void RegisterCosmosDbContainer(this IServiceCollection services)
    {
        services.RegisterSharedDatabaseServices();
        services.AddTransient<IPooledObjectPolicy<IDatabase>, CosmosDbDatabasePoolPolicy>();

        var container = new CosmosDbBuilder()
            .WithImage("mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:latest")
            .WithEnvironment(new Dictionary<string, string>
            {
                { "AZURE_COSMOS_EMULATOR_PARTITION_COUNT", "10" },
                { "AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE", "127.0.0.1" }
            })
            .WithPortBinding("8081", "8081")
            .Build();

        Utils.RunWithoutSynchronizationContext(() => container.StartAsync().Wait());
        services.AddSingleton(container);


        services.AddSingleton(_ =>
        {
            CosmosClientOptions cosmosClientOptions = new()
            {
                SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase },
                ConnectionMode = ConnectionMode.Gateway,
                RequestTimeout = TimeSpan.FromSeconds(5),
                HttpClientFactory = () => container.HttpClient
            };

            return new CosmosClient(container.GetConnectionString(), cosmosClientOptions);
        });
    }
}
