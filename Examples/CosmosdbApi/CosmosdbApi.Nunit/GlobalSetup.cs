using CosmosdbApi.Nunit.TestSetup;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.CosmosDb;
using TestExamplesDotnet;
using TestExamplesDotnet.Nunit;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.Children)]

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

        var cosmosDbContainer = new CosmosDbBuilder()
            .WithImage("mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:latest")
            .WithEnvironment(new Dictionary<string, string>
            {
                { "AZURE_COSMOS_EMULATOR_PARTITION_COUNT", "1" },
                { "AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE", "127.0.0.1" }
            })
            .WithPortBinding("8081", "8081")
            .Build();
        Utils.RunWithoutSynchronizationContext(() => cosmosDbContainer.StartAsync().Wait());

        services.AddSingleton(cosmosDbContainer);
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
