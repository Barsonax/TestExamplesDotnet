using System.Diagnostics;
using AngularAuth.Backend.Sut;
using AngularAuth.Playwright.TestSetup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestExamplesDotnet;
using TestExamplesDotnet.EntityFrameworkCore;
using TestExamplesDotnet.PostgreSql;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.Children)]

namespace AngularAuth.Playwright;

[SetUpFixture]
public class GlobalSetup
{
    internal static IServiceProvider Provider => _serviceProvider ?? throw new InvalidOperationException("GlobalSetup has not been run.");
    private static ServiceProvider? _serviceProvider;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        if (Debugger.IsAttached)
        {
            Environment.SetEnvironmentVariable("HEADED", "1");
        }

        InstallPlayWright();
        var services = new ServiceCollection();
        services.AddLogging(x => x.AddConsole());
        services.RegisterPostgreSqlContainer();
        services.AddScoped<AngularAuthSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
        _serviceProvider = services.BuildServiceProvider();
    }

    private static void InstallPlayWright()
    {
        var attempts = 0;

        while (true)
        {
            var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "--with-deps", "chromium" });

            if (exitCode != 0)
            {
                Console.WriteLine($"Failed to install playwright installation exited with code {exitCode}");

                if (attempts > 3)
                {
                    Assert.Fail();
                }
                else
                {
                    Console.WriteLine("Retrying playwright installation");
                }
            }
            else
            {
                return;
            }

            attempts++;
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
