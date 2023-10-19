using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Respawn;
using Testcontainers.PostgreSql;

namespace TestExamplesDotnet.PostgreSql;

public static class ServiceCollectionExtensions
{
    public static void RegisterPostgreSqlContainer(this IServiceCollection services)
    {
        services.RegisterSharedDatabaseServices();
        services.AddTransient<RespawnerOptions>(c => new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres
        });
        services.AddTransient<IPooledObjectPolicy<IDatabase>, PostgreSqlDatabasePoolPolicy>();

        var container = new PostgreSqlBuilder().Build();
        Utils.RunWithoutSynchronizationContext(() => container.StartAsync().Wait());
        services.AddSingleton(container);
    }
}
