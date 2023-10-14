using AwesomeApiTest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[SetUpFixture]
#pragma warning disable CA1050
// ReSharper disable once CheckNamespace this needs to be in the global namespace
public class AwesomeApiTestSetup
#pragma warning restore CA1050
{
    internal static IServiceProvider Provider => _serviceProvider;
    private static ServiceProvider _serviceProvider = null!;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var services = new ServiceCollection();

        services.AddLogging(x => x.AddConsole());
        services.RegisterMssqlContainer();
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
