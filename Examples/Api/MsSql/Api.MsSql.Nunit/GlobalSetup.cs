using Api.MsSql.Nunit.TestSetup;
using Api.MsSql.Sut;
using Microsoft.Extensions.DependencyInjection;
using TestExamplesDotnet;
using TestExamplesDotnet.Mssql;
using TestExamplesDotnet.Nunit;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.Children)]

namespace Api.MsSql.Nunit;

[SetUpFixture]
public class GlobalSetup
{
    internal static IServiceProvider Provider => _serviceProvider;
    private static ServiceProvider _serviceProvider = null!;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var services = new ServiceCollection();

        services.AddLogging(x => x.AddNunitLogging());
        services.RegisterMssqlContainer();
        services.AddScoped<ApiMsSqlSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _serviceProvider.DisposeAsync();
    }
}
