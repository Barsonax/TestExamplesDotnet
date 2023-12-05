using System.Reflection;
using AngularAuth.Backend.Sut;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestExamplesDotnet;

namespace AngularAuth.Playwright.TestSetup;

public sealed class AngularAuthSut : WebApplicationFactory<Program>
{
    private bool _disposed;
    private IHost? _host;

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    public override IServiceProvider Services
    {
        get
        {
            EnsureServer();
            return _host!.Services!;
        }
    }

    private readonly PooledDatabase _pooledDatabase;
    private readonly ILoggerProvider _loggerProvider;

    public AngularAuthSut(DatabasePool databasePool, ILoggerProvider loggerProvider)
    {
        _loggerProvider = loggerProvider;
        _pooledDatabase = databasePool.Get();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseUrls("http://127.0.0.1:0");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Production);
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "DbConnectionString", _pooledDatabase.ConnectionString }
            });
        });

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.Services.AddSingleton(_loggerProvider);
        });

        builder.ConfigureServices(services =>
        {
            services.ConfigureTestJwt();
        });

        // The next part is going to look hacky (and it is). The problem is by default the TestServer created by the WebApplicationFactory is not reachable by the Playwright browser.
        // To fix this we will use Kestrel instead of TestServer. Unfortunately WebApplicationFactory does not support this out of the box but with this hack it will.
        // Taken and modified from https://danieldonbavand.com/2022/06/13/using-playwright-with-the-webapplicationfactory-to-test-a-blazor-application/
        // Create the host for TestServer now before we
        // modify the builder to use Kestrel instead.
        var testHost = builder.Build();

        // Modify the host builder to use Kestrel instead
        // of TestServer so we can listen on a real address.
        builder.ConfigureWebHost(p => p.UseKestrel());

        // Create and start the Kestrel server before the test server,
        // otherwise due to the way the deferred host builder works
        // for minimal hosting, the server will not get "initialized
        // enough" for the address it is listening on to be available.
        _host = builder.Build();
        _host.Start();
        _pooledDatabase.EnsureInitialized(_host);

        // Extract the selected dynamic port out of the Kestrel server
        // and assign it onto the client options for convenience so it
        // "just works" as otherwise it'll be the default http://localhost
        // URL, which won't route to the Kestrel-hosted HTTP server.
        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses
            .Select(p => new Uri(p))
            .Last();

        // Return the host that uses TestServer, rather than the real one.
        // Otherwise the internals will complain about the host's server
        // not being an instance of the concrete type TestServer.
        testHost.Start();

        return testHost;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _pooledDatabase.DisposeAsync();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!_disposed)
        {
            if (disposing)
            {
                _host?.Dispose();
            }

            _disposed = true;
        }
    }

    private void EnsureServer()
    {
        typeof(WebApplicationFactory<Program>)
            .GetMethod("EnsureServer", BindingFlags.NonPublic | BindingFlags.Instance)
            !.Invoke(this, Array.Empty<object>());
    }

    public void SeedData(Action<BloggingContext> seedAction)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BloggingContext>();
        seedAction(context);
        context.SaveChanges();
    }

    public void AssertDatabase(Action<BloggingContext> seedAction)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BloggingContext>();
        seedAction(context);
    }
}
