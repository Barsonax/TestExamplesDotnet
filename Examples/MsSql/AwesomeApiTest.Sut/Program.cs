using AwesomeApiTest;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BloggingContext>(options =>
{
    options.UseSqlServer(builder.Configuration["DbConnectionString"]);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("blogs", (BloggingContext context) => TypedResults.Json(context.Blogs));

app.Run();

public partial class Program { }
