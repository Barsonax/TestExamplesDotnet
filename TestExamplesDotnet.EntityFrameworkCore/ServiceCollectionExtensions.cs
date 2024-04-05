using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestExamplesDotnet.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static void RegisterMigrationInitializer<TContext>(this IServiceCollection services)
        where TContext : DbContext, new()
    {
        services.AddSingleton<IDatabaseInitializer, DbContextMigrationInitializer<TContext>>();
    }
}
