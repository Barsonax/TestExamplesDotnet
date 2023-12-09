using Microsoft.Extensions.DependencyInjection;

namespace TestExamplesDotnet;

public static class ServiceCollectionExtensions
{
    public static void RegisterSharedDatabaseServices(this IServiceCollection services)
    {
        services.AddSingleton<DatabasePool>();
    }
}
