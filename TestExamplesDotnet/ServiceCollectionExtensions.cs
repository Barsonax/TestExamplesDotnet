using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeApiTest;

public static class ServiceCollectionExtensions
{
    public static void RegisterMigrationInitializer<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddTransient<IDatabaseInitializer, DbContextMigrationInitializer<TContext>>();
    }

    public static void RegisterSharedDatabaseServices(this IServiceCollection services)
    {
        services.AddSingleton<DatabasePool>();
    }
}
