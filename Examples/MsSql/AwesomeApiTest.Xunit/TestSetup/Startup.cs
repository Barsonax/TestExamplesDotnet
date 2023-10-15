using AwesomeApiTest.Sut;
using Microsoft.Extensions.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace AwesomeApiTest.Xunit.TestSetup;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.RegisterMssqlContainer();
        services.AddLogging(x => x.AddXunitOutput());

        services.AddScoped<AwesomeApiTestSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
    }
}
