using Microsoft.Extensions.ObjectPool;
using Respawn;
using Testcontainers.PostgreSql;

namespace TestExamplesDotnet.PostgreSql;

public sealed class PostgreSqlDatabasePoolPolicy : IPooledObjectPolicy<IDatabase>
{
    private readonly PostgreSqlContainer _container;
    private readonly IDatabaseInitializer _databaseInitializer;
    private readonly RespawnerOptions _respawnerOptions;
    private readonly IDataBaseNameGenerator _dataBaseNameGenerator;


    public PostgreSqlDatabasePoolPolicy(PostgreSqlContainer container, IDatabaseInitializer databaseInitializer, RespawnerOptions respawnerOptions, IDataBaseNameGenerator dataBaseNameGenerator)
    {
        _container = container;
        _databaseInitializer = databaseInitializer;
        _respawnerOptions = respawnerOptions;
        _dataBaseNameGenerator = dataBaseNameGenerator;
    }

    public IDatabase Create() => new PostgreSqlDatabase(_container, _databaseInitializer, _respawnerOptions, _dataBaseNameGenerator);

    public bool Return(IDatabase obj) => true;
}
