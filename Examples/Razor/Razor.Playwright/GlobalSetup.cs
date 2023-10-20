﻿using System.Diagnostics;
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
    internal static IServiceProvider Provider => _serviceProvider ?? throw new InvalidOperationException("GlobalSetup has not been run.");
    private static ServiceProvider? _serviceProvider;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        InstallPlayWright();
        var services = new ServiceCollection();
        services.AddLogging(x => x.AddConsole());
        services.RegisterPostgreSqlContainer();
        services.AddScoped<RazorSut>();
        services.RegisterMigrationInitializer<BloggingContext>();
        _serviceProvider = services.BuildServiceProvider();
    }

    private static void InstallPlayWright()
    {
        ProcessStartInfo startInfo = new()
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            FileName = "pwsh",
            Arguments = "playwright.ps1 install --with-deps"
        };

        using Process process = new()
        {
            StartInfo = startInfo,
        };
        process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
        process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);
        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            Console.WriteLine("Failed to install playwright.");
            Assert.Fail();
        }
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        if(_serviceProvider != null)
        {
            await _serviceProvider.DisposeAsync();
        }
    }
}
