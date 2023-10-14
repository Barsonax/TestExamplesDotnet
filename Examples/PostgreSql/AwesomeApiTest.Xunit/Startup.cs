using Microsoft.Extensions.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace AwesomeApiTest.Xunit;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.RegisterPostgreSqlContainer();
        services.AddLogging(x => x.AddXunitOutput());

        services.AddScoped<AwesomeApiTestSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
    }
}
