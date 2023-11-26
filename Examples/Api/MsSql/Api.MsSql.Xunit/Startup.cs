using Api.MsSql.Sut;
using Api.MsSql.Xunit.TestSetup;
using Microsoft.Extensions.DependencyInjection;
using TestExamplesDotnet;
using TestExamplesDotnet.Mssql;
using TestExamplesDotnet.Xunit;

namespace Api.MsSql.Xunit;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.RegisterMssqlContainer();
        services.AddLogging(x => x.AddXunitLogging());

        services.AddScoped<ApiMsSqlSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
    }
}
