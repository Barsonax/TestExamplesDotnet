using System.Text;
using Api.MsSql.Sut;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
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
    public async Task MigrationsUpAndDown_NoErrors()
    {
        await using DbContext context = CreateDbContext();
        var migrations = GenerateMigrationScripts(context);

        var databaseName = "MigrationSeparateScriptsTest";
        await _databaseContainer.CreateDatabase(databaseName);
        var migrator = new SqlMigrator(_databaseContainer, _logger, databaseName);
        foreach (var migration in migrations)
        {
            await migrator.Up(migration);
        }

        foreach (var migration in migrations.Reverse())
        {
            await migrator.Down(migration);
        }
    }

    private static DbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BloggingContext>();
        options.UseSqlServer();
        var context = new BloggingContext(options.Options);
        return context;
    }

    private static MigrationScript[] GenerateMigrationScripts(DbContext context)
    {
        var migrations = context.Database.GetMigrations().ToArray();
        var migrator = context.Database.GetInfrastructure().GetRequiredService<IMigrator>();

        var migrationScripts = new List<MigrationScript>();
        string? previousMigration = null;

        foreach (string migrationName in migrations)
        {
            migrationScripts.Add(GenerateScript(migrator, previousMigration, migrationName));
            previousMigration = migrationName;
        }

        return migrationScripts.ToArray();
    }

    private static MigrationScript GenerateScript(IMigrator migrator, string? fromMigration, string toMigration)
    {
        var upScript = migrator.GenerateScript(fromMigration, toMigration, MigrationsSqlGenerationOptions.Idempotent);
        var downScript = migrator.GenerateScript(toMigration, fromMigration, MigrationsSqlGenerationOptions.Idempotent);
        return new MigrationScript(fromMigration ?? "empty", toMigration, upScript, downScript);
    }
}

public static class MsSqlContainerExtensions
{
    public static async Task<ExecResult> ExecScriptAsync(this MsSqlContainer container, string scriptContent, string database, CancellationToken ct = default)
    {
        var scriptFilePath = string.Join("/", string.Empty, "tmp", Guid.NewGuid().ToString("D"), Path.GetRandomFileName());

        await container.CopyAsync(Encoding.Default.GetBytes(scriptContent), scriptFilePath, Unix.FileMode644, ct)
            .ConfigureAwait(false);

        var connectionString = ParseConnectionString(container.GetConnectionString());

        return await container
            .ExecAsync(new[] { "/opt/mssql-tools/bin/sqlcmd", "-b", "-r", "1", "-U", connectionString.UserId, "-P", connectionString.Password, "-d", database, "-i", scriptFilePath }, ct)
            .ConfigureAwait(false);
    }

    private record SqlContainerConnectionString(string UserId, string Password);
    private static SqlContainerConnectionString ParseConnectionString(string connectionString)
    {
        var dic = connectionString
            .Split(';')
            .Select(x => x.Split('='))
            .ToDictionary(x => x[0], x => x[1]);

        return new SqlContainerConnectionString(dic["User Id"], dic["Password"]);
    }

    public static async Task CreateDatabase(this MsSqlContainer container, string name)
    {
        await container.ExecScriptAsync($"CREATE DATABASE {name}");
    }
}

public class SqlMigrator
{
    private readonly MsSqlContainer _container;
    private readonly ILogger _logger;
    private readonly string _databaseName;

    public SqlMigrator(MsSqlContainer container, ILogger logger, string databaseName)
    {
        _container = container;
        _logger = logger;
        _databaseName = databaseName;
    }

    public async Task Up(MigrationScript script) => await ExecuteMigration(script.UpScript, script.FromMigration, script.ToMigration);

    public async Task Down(MigrationScript script) => await ExecuteMigration(script.DownScript, script.ToMigration, script.FromMigration);

    private async Task ExecuteMigration(string script, string from, string to)
    {
        _logger.LogInformation("Migrating from {FromMigration} to {ToMigration}", from, to);
        try
        {
            // This will use sqlcmd to execute the script. The behavior is different from executing the migration from code, for instance the script will fail if a column is referenced that does not exist.
            await _container.ExecScriptAsync(script, _databaseName);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Error while migrating from {to} to {from}", e);
        }
    }
}

public record MigrationScript(string FromMigration, string ToMigration, string UpScript, string DownScript);
