using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestExamplesDotnet.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static void RegisterMigrationInitializer<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddTransient<IDatabaseInitializer, DbContextMigrationInitializer<TContext>>();
    }
}
