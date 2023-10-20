using Microsoft.EntityFrameworkCore;
using Razor.PostgreSql.Sut;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BloggingContext>(options =>
{
    options.UseNpgsql(builder.Configuration["DbConnectionString"]);
});

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

namespace Razor.PostgreSql.Sut
{
    public partial class Program { }
}
