using Microsoft.Extensions.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace AwesomeApiTest.Xunit;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.RegisterPostgreSqlContainer();
        services.AddLogging(x => x.AddXunitOutput());
        
        services.AddScoped<AwesomeApiTestSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
    }
}