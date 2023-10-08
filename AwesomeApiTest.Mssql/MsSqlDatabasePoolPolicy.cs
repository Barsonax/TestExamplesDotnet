using Microsoft.Extensions.ObjectPool;
using Respawn;
using Testcontainers.MsSql;

namespace AwesomeApiTest;

public class MsSqlDatabasePoolPolicy : IPooledObjectPolicy<IDatabase>
{
    private readonly MsSqlContainer _container;
    private readonly IDatabaseInitializer _databaseInitializer;
    private readonly RespawnerOptions _respawnerOptions;


    public MsSqlDatabasePoolPolicy(MsSqlContainer container, IDatabaseInitializer databaseInitializer, RespawnerOptions respawnerOptions)
    {
        _container = container;
        _databaseInitializer = databaseInitializer;
        _respawnerOptions = respawnerOptions;
    }

    public IDatabase Create() => new MsSqlDatabase(_container, _databaseInitializer, _respawnerOptions);

    public bool Return(IDatabase obj) => true;
}