using Api.MsSql.Sut;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Testcontainers.CosmosDb;

namespace CosmosdbApi.Nunit.TestSetup;

public sealed class CosmosdbApiSut : WebApplicationFactory<Program>
{
    private readonly ILoggerProvider _loggerProvider;
    private readonly CosmosDbContainer _cosmosDbContainer;

    public CosmosdbApiSut(ILoggerProvider loggerProvider)
    {
        _cosmosDbContainer = new CosmosDbBuilder()
            .WithImage("mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:latest")
            .Build();
        _loggerProvider = loggerProvider;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {


        builder.UseEnvironment(Environments.Production);
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "DbConnectionString", _cosmosDbContainer.GetConnectionString() }
            });
        });

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.Services.AddSingleton(_loggerProvider);
        });

        var app = base.CreateHost(builder);

        return app;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _cosmosDbContainer.DisposeAsync();
    }

    public async Task SeedDataAsync(Func<CosmosClient, Task> seedAction)
    {
        using var scope = Services.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<CosmosClient>();
        await seedAction(client);
    }
}
