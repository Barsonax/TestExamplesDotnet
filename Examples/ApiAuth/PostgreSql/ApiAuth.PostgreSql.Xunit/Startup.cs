using ApiAuth.PostgreSql.Sut;
using ApiAuth.PostgreSql.Xunit.TestSetup;
using Microsoft.Extensions.DependencyInjection;
using TestExamplesDotnet;
using TestExamplesDotnet.PostgreSql;
using Xunit.DependencyInjection.Logging;

namespace ApiAuth.PostgreSql.Xunit;

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
