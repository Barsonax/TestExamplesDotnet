using Api.PostgreSql.Sut;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BloggingContext>(options =>
{
    options.UseNpgsql(builder.Configuration["DbConnectionString"]);
});

var app = builder.Build();

app.MapGet("blogs", (BloggingContext context) => TypedResults.Json(context.Blogs));

app.Run();

namespace Api.PostgreSql.Sut
{
    public partial class Program { }
}
