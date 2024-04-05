using Api.PostgreSql.Sut;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestExamplesDotnet;

namespace Api.PostgreSql.Nunit.TestSetup;

public sealed class ApiPostgreSqlSut : WebApplicationFactory<Program>
{
    private readonly PooledDatabase _pooledDatabase;
    private readonly ILoggerProvider _loggerProvider;

    public ApiPostgreSqlSut(DatabasePool databasePool, ILoggerProvider loggerProvider)
    {
        _loggerProvider = loggerProvider;
        _pooledDatabase = databasePool.Get();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Production);
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "DbConnectionString", _pooledDatabase.ConnectionString }
            });
        });

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.Services.AddSingleton(_loggerProvider);
        });

        var app = base.CreateHost(builder);
        _pooledDatabase.EnsureDatabaseIsReadyForTest(app);

        return app;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _pooledDatabase.Dispose();
    }

    public void SeedData(Action<BloggingContext> seedAction)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BloggingContext>();
        seedAction(context);
        context.SaveChanges();
    }
}
