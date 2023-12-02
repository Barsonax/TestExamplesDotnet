using Microsoft.EntityFrameworkCore;
using VueAuth.Sut;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BloggingContext>(options =>
{
    options.UseNpgsql(builder.Configuration["DbConnectionString"]);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<BloggingContext>();
    context.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("blogs", (BloggingContext context) => TypedResults.Json(context.Blogs));

app.Run();

namespace VueAuth.Sut
{
    public partial class Program { }
}
