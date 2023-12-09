using Microsoft.Azure.Cosmos;

namespace CosmosdbApi.Sut;

public class CosmosDbInitializer(CosmosClient client, IConfiguration configuration)
{
    public async Task InitializeAsync()
    {
        var databaseResponse =  await client.CreateDatabaseAsync(configuration["Database"]);
        await databaseResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties
        {
            Id = "Blogs",
            PartitionKeyPath = "/Url"
        });
    }
}
