using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace ApiAuth.PostgreSql.Nunit.TestSetup;

public static class TestJwtGenerator
{
    private const string Audience = "TestUsers";
    private static readonly string _issuer = Guid.NewGuid().ToString();
    private static readonly SecurityKey _securityKey;
    private static readonly SigningCredentials _signingCredentials;

    private static readonly JwtSecurityTokenHandler _sTokenHandler = new();

    static TestJwtGenerator()
    {
        _securityKey = new SymmetricSecurityKey("A1B2C3D4E5F6A1B2C3D4E5F6"u8.ToArray()) { KeyId = Guid.NewGuid().ToString() };
        _signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
    }

    public static string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        return _sTokenHandler.WriteToken(new JwtSecurityToken(_issuer, Audience, claims, null, DateTime.UtcNow.AddMinutes(20), _signingCredentials));
    }

    public static IServiceCollection ConfigureTestJwt(this IServiceCollection services)
    {
        services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Configuration = new OpenIdConnectConfiguration
            {
                Issuer = _issuer,
                SigningKeys = { _securityKey }
            };
            options.Audience = Audience;
        });

        return services;
    }
}
