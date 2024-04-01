using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Respawn;
using Testcontainers.MsSql;

namespace TestExamplesDotnet.Mssql;

public static class ServiceCollectionExtensions
{
    public static void RegisterMssqlContainer(this IServiceCollection services)
    {
        services.RegisterSharedDatabaseServices();
        services.AddTransient<RespawnerOptions>(c => new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer
        });
        services.AddTransient<IPooledObjectPolicy<IDatabase>, MsSqlDatabasePoolPolicy>();

        var container = new MsSqlBuilder()
            .WithReuse(true)
            .Build();
        Utils.RunWithoutSynchronizationContext(() => container.StartAsync().Wait());
        services.AddSingleton(container);
    }
}
