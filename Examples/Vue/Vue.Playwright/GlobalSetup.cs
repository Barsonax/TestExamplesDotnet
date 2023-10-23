using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestExamplesDotnet;
using TestExamplesDotnet.PostgreSql;
using Vue.Backend.Sut;
using Vue.Playwright.TestSetup;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.Children)]

namespace Vue.Playwright;

[SetUpFixture]
public class GlobalSetup
{
    internal static IServiceProvider Provider => _serviceProvider ?? throw new InvalidOperationException("GlobalSetup has not been run.");
    private static ServiceProvider? _serviceProvider;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        InstallPlayWright();
        var services = new ServiceCollection();
        services.AddLogging(x => x.AddConsole());
        services.RegisterPostgreSqlContainer();
        services.AddScoped<VueSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
        _serviceProvider = services.BuildServiceProvider();
    }

    private static void InstallPlayWright()
    {
        var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "--with-deps", "chromium" });

        if (exitCode != 0)
        {
            Console.WriteLine($"Failed to run  Playwright exited with code {exitCode}");
            Assert.Fail();
        }
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        if (_serviceProvider != null)
        {
            await _serviceProvider.DisposeAsync();
        }
    }
}
