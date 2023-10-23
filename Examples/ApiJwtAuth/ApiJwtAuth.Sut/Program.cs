using ApiJwtAuth.Sut;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

var specialPermissionPolicy = new AuthorizationPolicyBuilder()
    .RequireClaim("SpecialPermission")
    .RequireAuthenticatedUser()
    .Build();

app.MapGet("blogs", (BloggingContext context) => TypedResults.Json(context.Blogs)).RequireAuthorization(specialPermissionPolicy);

app.Run();

namespace ApiJwtAuth.Sut
{
    public partial class Program { }
}
