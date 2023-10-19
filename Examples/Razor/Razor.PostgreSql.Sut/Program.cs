using AwesomeApiTest.Sut;
using Microsoft.EntityFrameworkCore;

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

public partial class Program { }
