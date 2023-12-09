using Api.MsSql.Sut;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Testcontainers.CosmosDb;
using TestExamplesDotnet;

namespace CosmosdbApi.Nunit.TestSetup;

public sealed class CosmosdbApiSut : WebApplicationFactory<Program>
{
    private readonly PooledDatabase _pooledDatabase;
    private readonly ILoggerProvider _loggerProvider;
    private readonly CosmosDbContainer _container;

    public CosmosdbApiSut(DatabasePool databasePool, ILoggerProvider loggerProvider, CosmosDbContainer container)
    {
        _loggerProvider = loggerProvider;
        _container = container;
        _pooledDatabase = databasePool.Get();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Production);
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "DbConnectionString", _container.GetConnectionString() },
                { "Database", _pooledDatabase.ConnectionString }
            });
        });

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.Services.AddSingleton(_loggerProvider);
        });

        var app = base.CreateHost(builder);
        _pooledDatabase.EnsureInitialized(app);

        return app;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _pooledDatabase.DisposeAsync();
    }

    public async Task SeedDataAsync(Func<Database, Task> seedAction)
    {
        using var scope = Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<Database>();
        await seedAction(client);
    }
}
