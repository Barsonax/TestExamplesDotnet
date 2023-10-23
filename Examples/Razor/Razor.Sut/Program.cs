using Microsoft.EntityFrameworkCore;
using Razor.PostgreSql.Sut;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BloggingContext>(options =>
{
    options.UseNpgsql(builder.Configuration["DbConnectionString"]);
});

builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<BloggingContext>();
    context.Database.Migrate();
}

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

namespace Razor.PostgreSql.Sut
{
    public partial class Program { }
}
