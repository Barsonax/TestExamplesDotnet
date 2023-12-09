using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.ObjectPool;

namespace TestExamplesDotnet.CosmosDb;

public sealed class CosmosDbDatabasePoolPolicy : IPooledObjectPolicy<IDatabase>
{
    private readonly CosmosClient _cosmosClient;
    
    public CosmosDbDatabasePoolPolicy(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    public IDatabase Create() => new CosmosDbDatabase(_cosmosClient);

    public bool Return(IDatabase obj) => true;
}
