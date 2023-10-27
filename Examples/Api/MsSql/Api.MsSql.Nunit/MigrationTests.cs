using Api.MsSql.Sut;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace Api.MsSql.Nunit;

public class MigrationTests
{
#pragma warning disable NUnit1032
    private MsSqlContainer _databaseContainer = null!;
    private ILogger<MigrationTests> _logger = null!;
#pragma warning restore NUnit1032
    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        _databaseContainer = _scope.ServiceProvider.GetRequiredService<MsSqlContainer>();
        _logger = _scope.ServiceProvider.GetRequiredService<ILogger<MigrationTests>>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }

    [Test]
    public async Task MigrationsUpAndDown_SeparateScripts_NoErrors()
    {
        var databaseName = "MigrationSeparateScriptsTest";
        await CreateDatabase(databaseName);

        await using DbContext context = CreateDbContext();
        var pendingMigrations = context.Database.GetMigrations().ToArray();
        var migrations = GenerateMigrationScripts(context, pendingMigrations);

        foreach ((string from, string to, string upScript, string _) in migrations)
        {
            await ExecuteMigrationWithLogging(from, to, upScript, _logger);
        }

        foreach ((string from, string to, string _, string downScript) in migrations.Reverse())
        {
            await ExecuteMigrationWithLogging(to, from, downScript, _logger);
        }
    }

    [Test]
    public async Task MigrationsUpAndDown_SingleScript_NoErrors()
    {
        var databaseName = "MigrationSingleScriptsTest";
        await CreateDatabase(databaseName);

        await using DbContext context = CreateDbContext();
        var latestMigration = context.Database.GetMigrations().Last();
        var migrator = context.Database.GetInfrastructure().GetRequiredService<IMigrator>();

        var upScript = GenerateScript(migrator, null, latestMigration);
        var downScript = GenerateScript(migrator, latestMigration, Migration.InitialDatabase);

        await ExecuteMigrationWithLogging("empty", latestMigration, upScript, _logger);
        await ExecuteMigrationWithLogging(latestMigration, "empty", downScript, _logger);
    }

    private async Task ExecuteMigrationWithLogging(string fromMigration, string toMigration, string script, ILogger logger)
    {
        logger.LogInformation("Testing migrating from {FromMigration} to {ToMigration}", fromMigration, toMigration);

        try
        {
            // This will use sqlcmd to execute the script. The behavior is different from executing the migration from code, for instance the script will fail if a column is referenced that does not exist.
            await _databaseContainer.ExecScriptAsync(script);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Error while migrating from {fromMigration} to {toMigration}", e);
        }
    }

    private async Task CreateDatabase(string name)
    {
        await _databaseContainer.ExecScriptAsync($"CREATE DATABASE {name}");
    }

    private static DbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BloggingContext>();
        options.UseSqlServer();
        var context = new BloggingContext(options.Options);
        return context;
    }

    private static (string fromMigration, string toMigration, string upScript, string downScript)[] GenerateMigrationScripts(DbContext context, string[] migrations)
    {
        var migrator = context.Database.GetInfrastructure().GetRequiredService<IMigrator>();

        var migrationScripts = new List<(string fromMigration, string toMigration, string upScript, string downScript)>();
        string previousMigration = null;

        foreach (string migrationName in migrations)
        {
            var upScript = migrator.GenerateScript(previousMigration, migrationName, MigrationsSqlGenerationOptions.Idempotent)
                .Replace($"GO", "", StringComparison.Ordinal);

            string downScript = GenerateScript(migrator, migrationName, previousMigration);
            migrationScripts.Add((previousMigration ?? "empty", migrationName, upScript, downScript));
            previousMigration = migrationName;
        }

        return migrationScripts.ToArray();
    }

    private static string GenerateScript(IMigrator migrator, string fromMigration, string toMigration)
    {
        var downScript = migrator.GenerateScript(fromMigration, toMigration, MigrationsSqlGenerationOptions.Idempotent)
            .Replace($"GO", "", StringComparison.Ordinal);

        return downScript;
    }
}
