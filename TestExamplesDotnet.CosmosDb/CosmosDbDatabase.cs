using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;

namespace TestExamplesDotnet.CosmosDb;

public sealed class CosmosDbDatabase : IDatabase
{
    private readonly CosmosClient _cosmosClient;
    private bool _initialized;
    public string ConnectionString { get; } = Guid.NewGuid().ToString();

    public CosmosDbDatabase(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    public void Initialize(IHost host)
    {
        if (!_initialized)
        {
            _initialized = true;
        }
    }

    public async ValueTask Clean()
    {
        if (_initialized)
        {
            await _cosmosClient.GetDatabase(ConnectionString).DeleteAsync();
        }
    }
}
