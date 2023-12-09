using CosmosdbApi.Nunit.TestSetup;
using Microsoft.Extensions.DependencyInjection;
using TestExamplesDotnet.CosmosDb;
using TestExamplesDotnet.Nunit;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.None)] // Unfortunately the cosmos db emulator will give 503 errors if we stress it too much

namespace CosmosdbApi.Nunit;

[SetUpFixture]
public class GlobalSetup
{
    internal static IServiceProvider Provider => _serviceProvider;
    private static ServiceProvider _serviceProvider = null!;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var services = new ServiceCollection();

        services.RegisterCosmosDbContainer();
        services.AddLogging(x => x.AddNunitLogging());
        services.AddScoped<CosmosdbApiSut>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _serviceProvider.DisposeAsync();
    }
}
