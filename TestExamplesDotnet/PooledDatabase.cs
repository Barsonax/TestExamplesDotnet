using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;

namespace TestExamplesDotnet;

public sealed class PooledDatabase : IDisposable
{
    private readonly IDatabase _database;

    private readonly ObjectPool<IDatabase> _pool;

    internal PooledDatabase(ObjectPool<IDatabase> pool)
    {
        _pool = pool;
        _database = pool.Get();
    }

    public string ConnectionString => _database.ConnectionString;

    public void EnsureDatabaseIsReadyForTest(IHost host)
    {
        _database.EnsureInitialized(host);
        // Clean the database before and not after the test so that after a test is run you can inspect the database.
        Utils.RunWithoutSynchronizationContext(() =>
        {
            _database.Clean().GetAwaiter().GetResult();
        });
    }

    public void Dispose()
    {
        _pool.Return(_database);
    }
}
