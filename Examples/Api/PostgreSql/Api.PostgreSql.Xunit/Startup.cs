using Api.PostgreSql.Sut;
using Api.PostgreSql.Xunit.TestSetup;
using Microsoft.Extensions.DependencyInjection;
using TestExamplesDotnet;
using TestExamplesDotnet.PostgreSql;
using TestExamplesDotnet.Xunit;

namespace Api.PostgreSql.Xunit;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.RegisterPostgreSqlContainer();
        services.AddLogging(x => x.AddXunitLogging());

        services.AddScoped<ApiPostgreSqlSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
    }
}
