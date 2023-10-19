using Microsoft.Extensions.Hosting;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace AwesomeApiTest;

public sealed class PostgreSqlDatabase : IDatabase
{
    private readonly IDatabaseInitializer _databaseInitializer;
    private readonly RespawnerOptions _respawnerOptions;
    private Respawner? _respawner;
    private bool _initialized;
    public string ConnectionString { get; }

    public PostgreSqlDatabase(PostgreSqlContainer container, IDatabaseInitializer databaseInitializer, RespawnerOptions respawnerOptions)
    {
        _databaseInitializer = databaseInitializer;
        _respawnerOptions = respawnerOptions;
        ConnectionString = $"Host=127.0.0.1;Port={container.GetMappedPublicPort(5432)};Database={Guid.NewGuid()};Username=postgres;Password=postgres;Include Error Detail=true";
    }

    public void Initialize(IHost host)
    {
        if (!_initialized)
        {
            _databaseInitializer.Initialize(host);
            _initialized = true;
        }
    }

    public async ValueTask Clean()
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        _respawner ??= await Respawner.CreateAsync(conn, _respawnerOptions);

        await _respawner.ResetAsync(conn);
    }
}
