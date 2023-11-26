using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestExamplesDotnet;

public sealed class DbContextMigrationInitializer<TDbContext> : IDatabaseInitializer
    where TDbContext : DbContext
{
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
}
