﻿using Api.MsSql.Sut;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Migrations.MsSql.EntityFrameworkCore.Nunit.TestSetup;
using Migrations.MsSql.EntityFrameworkCore.Sut;
using TestExamplesDotnet;
using TestExamplesDotnet.Mssql;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.Children)]

namespace Migrations.MsSql.EntityFrameworkCore.Nunit;

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
