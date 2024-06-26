﻿using System.Net.Http.Headers;
using System.Security.Claims;
using ApiJwtAuth.Sut;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestExamplesDotnet;

namespace ApiJwtAuth.Nunit.TestSetup;

public sealed class ApiJwtAuthSut : WebApplicationFactory<Program>
{
    private readonly PooledDatabase _pooledDatabase;
    private readonly ILoggerProvider _loggerProvider;

    public ApiJwtAuthSut(DatabasePool databasePool, ILoggerProvider loggerProvider)
    {
        _loggerProvider = loggerProvider;
        _pooledDatabase = databasePool.Get();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Production);
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "DbConnectionString", _pooledDatabase.ConnectionString },
                { "Logging:LogLevel:Microsoft.AspNetCore.Routing", "Information" },
            });
        });

        builder.ConfigureServices(services =>
        {
            services.ConfigureTestJwt();
        });

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.Services.AddSingleton(_loggerProvider);
        });

        var app = base.CreateHost(builder);
        _pooledDatabase.EnsureDatabaseIsReadyForTest(app);

        return app;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _pooledDatabase.Dispose();
    }

    public void SeedData(Action<BloggingContext> seedAction)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BloggingContext>();
        seedAction(context);
        context.SaveChanges();
    }

    public HttpClient CreateAuthorizedClient(params Claim[] claims)
    {
        var client = CreateClient();
        var jwt = TestJwtGenerator.GenerateJwtToken(claims);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        return client;
    }
}
