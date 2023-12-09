using Api.PostgreSql.Sut;
using Api.PostgreSql.Xunit.TestSetup;
using Microsoft.Extensions.DependencyInjection;
using TestExamplesDotnet;
using TestExamplesDotnet.EntityFrameworkCore;
using TestExamplesDotnet.PostgreSql;
using Xunit.DependencyInjection.Logging;

namespace Api.PostgreSql.Xunit;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.RegisterPostgreSqlContainer();
        services.AddLogging(x => x.AddXunitOutput());

        services.AddScoped<ApiPostgreSqlSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
    }
}
