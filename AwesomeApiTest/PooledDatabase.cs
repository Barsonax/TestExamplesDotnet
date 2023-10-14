using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;

namespace AwesomeApiTest;

public sealed class PooledDatabase : IAsyncDisposable
{
    private readonly IDatabase _database;

    private readonly ObjectPool<IDatabase> _pool;

    public PooledDatabase(ObjectPool<IDatabase> pool)
    {
        _pool = pool;
        _database = pool.Get();
    }

    public string ConnectionString => _database.ConnectionString;

    public void EnsureInitialized(IHost host)
    {
        _database.Initialize(host);
    }

    public async ValueTask DisposeAsync()
    {
        await _database.Clean();
        _pool.Return(_database);
    }
}
