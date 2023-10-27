using System.Text;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Testcontainers.MsSql;

namespace TestExamplesDotnet.Mssql;

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

    private sealed record SqlContainerConnectionString(string UserId, string Password);
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
