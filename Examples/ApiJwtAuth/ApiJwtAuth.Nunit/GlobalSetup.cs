using ApiJwtAuth.Nunit.TestSetup;
using ApiJwtAuth.Sut;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestExamplesDotnet;
using TestExamplesDotnet.PostgreSql;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.Children)]

namespace ApiJwtAuth.Nunit;

[SetUpFixture]
public class GlobalSetup
{
    internal static IServiceProvider Provider => _serviceProvider;
    private static ServiceProvider _serviceProvider = null!;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var services = new ServiceCollection();

        services.AddLogging(x => x.AddConsole());
        services.RegisterPostgreSqlContainer();
        services.AddScoped<ApiJwtAuthSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _serviceProvider.DisposeAsync();
    }
}
