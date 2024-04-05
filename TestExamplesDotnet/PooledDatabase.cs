﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;

namespace TestExamplesDotnet;

public sealed class PooledDatabase : IAsyncDisposable
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
        _database.Initialize(host);
        _database.Clean().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        _pool.Return(_database);
    }
}
