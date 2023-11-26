using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace TestExamplesDotnet;

public record MigrationScript(string FromMigration, string ToMigration, string UpScript, string DownScript)
{
    public override string ToString() => ToMigration;
}

public static class DbContextExtensions
{
    public static MigrationScript[] GenerateMigrationScripts(this DbContext context)
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
