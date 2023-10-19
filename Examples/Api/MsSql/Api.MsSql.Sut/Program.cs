using Api.MsSql.Sut;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BloggingContext>(options =>
{
    options.UseSqlServer(builder.Configuration["DbConnectionString"]);
});

var app = builder.Build();

app.MapGet("blogs", (BloggingContext context) => TypedResults.Json(context.Blogs));

app.Run();

namespace Api.MsSql.Sut
{
    public partial class Program { }
}
