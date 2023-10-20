using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Razor.Playwright.TestSetup;
using Razor.PostgreSql.Sut;
using TestExamplesDotnet;
using TestExamplesDotnet.PostgreSql;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.Children)]

namespace Razor.Playwright;

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
        services.AddScoped<RazorSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _serviceProvider.DisposeAsync();
    }
}
