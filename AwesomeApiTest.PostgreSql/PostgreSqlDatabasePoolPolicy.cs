using Microsoft.Extensions.ObjectPool;
using Respawn;
using Testcontainers.PostgreSql;

namespace AwesomeApiTest;

public class PostgreSqlDatabasePoolPolicy : IPooledObjectPolicy<IDatabase>
{
    private readonly PostgreSqlContainer _container;
    private readonly IDatabaseInitializer _databaseInitializer;
    private readonly RespawnerOptions _respawnerOptions;


    public PostgreSqlDatabasePoolPolicy(PostgreSqlContainer container, IDatabaseInitializer databaseInitializer, RespawnerOptions respawnerOptions)
    {
        _container = container;
        _databaseInitializer = databaseInitializer;
        _respawnerOptions = respawnerOptions;
    }

    public IDatabase Create() => new PostgreSqlDatabase(_container, _databaseInitializer, _respawnerOptions);

    public bool Return(IDatabase obj) => true;
}