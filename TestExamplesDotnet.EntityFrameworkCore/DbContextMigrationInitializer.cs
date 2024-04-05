using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestExamplesDotnet;

public sealed class DbContextMigrationInitializer<TDbContext> : IDatabaseInitializer
    where TDbContext : DbContext, new()
{
    private int _counter;
    private readonly Lazy<string> _databaseHash;

    public DbContextMigrationInitializer()
    {
        _databaseHash = new Lazy<string>(() =>
        {
            using var context = new TDbContext();
            var createScript = context.GetService<IRelationalDatabaseCreator>().GenerateCreateScript();
#pragma warning disable CA5351
            return Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(createScript)));
#pragma warning restore CA5351
        });
    }

    public void Initialize(IHost app)
    {
        using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();

        var configurationRoot = (IConfigurationRoot)app.Services.GetRequiredService<IConfiguration>();
        configurationRoot["Logging:LogLevel:Microsoft.EntityFrameworkCore"] = LogLevel.Warning.ToString();
        configurationRoot.Reload();
        context.Database.EnsureCreated();
        configurationRoot["Logging:LogLevel:Microsoft.EntityFrameworkCore"] = LogLevel.Information.ToString();
        configurationRoot.Reload();
    }

    public string GetUniqueDataBaseName()
    {
        var counter = Interlocked.Increment(ref _counter);
        return $"{_databaseHash.Value}_{counter}";
    }
}
