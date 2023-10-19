using Api.MsSql.Sut;
using Api.MsSql.Xunit.TestSetup;
using Microsoft.Extensions.DependencyInjection;
using TestExamplesDotnet;
using TestExamplesDotnet.Mssql;
using Xunit.DependencyInjection.Logging;

namespace Api.MsSql.Xunit;

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
