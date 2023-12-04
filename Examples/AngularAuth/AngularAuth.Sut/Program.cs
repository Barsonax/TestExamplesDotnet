using AngularAuth.Sut;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    WebRootPath = "wwwroot/browser"
});

builder.Services.AddDbContext<BloggingContext>(options =>
{
    options.UseNpgsql(builder.Configuration["DbConnectionString"]);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<BloggingContext>();
    context.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("blogs", (BloggingContext context) => TypedResults.Json(context.Blogs)).RequireAuthorization();

app.Run();

namespace AngularAuth.Sut
{
    public partial class Program { }
}
