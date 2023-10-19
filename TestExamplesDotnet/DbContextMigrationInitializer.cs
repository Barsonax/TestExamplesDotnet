using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestExamplesDotnet;

public sealed class DbContextMigrationInitializer<TDbContext> : IDatabaseInitializer
    where TDbContext : DbContext
{
    public void Initialize(IHost app)
    {
        using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
        context.Database.Migrate();
    }
}
