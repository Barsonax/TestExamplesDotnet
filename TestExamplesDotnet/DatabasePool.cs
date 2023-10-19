using Microsoft.Extensions.ObjectPool;

namespace TestExamplesDotnet;

public sealed class DatabasePool
{
    private readonly ObjectPool<IDatabase> _pool;

    public DatabasePool(IPooledObjectPolicy<IDatabase> policy)
    {
        var poolFactory = new DefaultObjectPoolProvider();
        _pool = poolFactory.Create(policy);
    }

    public PooledDatabase Get() => new(_pool);
}
