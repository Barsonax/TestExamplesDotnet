using Microsoft.EntityFrameworkCore;
using Migrations.MsSql.EntityFrameworkCore.Sut;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BloggingContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<BloggingContext>();
    context.Database.Migrate();
}

app.Run();

namespace Migrations.MsSql.EntityFrameworkCore.Sut
{
    public partial class Program { }
}
