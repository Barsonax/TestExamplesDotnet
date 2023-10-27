using Microsoft.Extensions.Logging;
using Testcontainers.MsSql;

namespace TestExamplesDotnet.Mssql;

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
