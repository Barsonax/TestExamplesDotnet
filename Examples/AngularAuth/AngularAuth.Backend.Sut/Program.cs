using AngularAuth.Backend.Sut;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    WebRootPath = "wwwroot/browser"
});

builder.Services.AddDbContext<BloggingContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();
builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(options => options.AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .Build());

    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var context = serviceScope.ServiceProvider.GetRequiredService<BloggingContext>();
    context.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/api/blogs", (BloggingContext context) => TypedResults.Json(context.Blogs)).RequireAuthorization();

app.Run();

namespace AngularAuth.Backend.Sut
{
    public partial class Program { }
}
