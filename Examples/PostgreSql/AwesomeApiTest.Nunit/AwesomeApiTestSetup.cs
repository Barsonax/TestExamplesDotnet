using AwesomeApiTest.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AwesomeApiTest.Nunit;

[SetUpFixture]
public class AwesomeApiTestSetup
{
    internal static IServiceProvider Provider => _serviceProvider;
    private static ServiceProvider _serviceProvider = null!;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var services = new ServiceCollection();

        services.AddLogging(x => x.AddConsole());
        services.RegisterPostgreSqlContainer();
        services.AddScoped<AwesomeApiTestSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _serviceProvider.DisposeAsync();
    }
}
