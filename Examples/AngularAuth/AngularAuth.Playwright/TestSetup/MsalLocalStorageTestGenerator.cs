using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace AngularAuth.Playwright.TestSetup;

public static class MsalLocalStorageTestGenerator
{
    private static JsonSerializerOptions JsonSerializerOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
    public static BrowserStorageStateLocalStorage[] Generate(IEnumerable<Claim> claims)
    {
        var environment = "login.windows.net";
        var clientId = "foobar";
        var localAccountId = Guid.NewGuid().ToString();
        var tenantId = Guid.NewGuid().ToString();
        var homeAccountId = $"{localAccountId}.{tenantId}";
        var jwtToken = TestJwtGenerator.GenerateJwtToken(claims);

        var accessToken = new MsalAccessToken
        {
            ClientId = clientId,
            HomeAccountId = homeAccountId,
            Realm = tenantId,
            Secret = jwtToken,
            Target = "User.Read"
        };

        var accountInfo = new MsalAccountInfo
        {
            HomeAccountId = homeAccountId,
            LocalAccountId = localAccountId,
            Realm = tenantId,
            TenantProfiles = new MsalTenantProfile[]
            {
                new()
                {
                    LocalAccountId = localAccountId,
                    TenantId = tenantId
                }
            },
        };

        return new BrowserStorageStateLocalStorage[]
        {
            new() { Name = $"{localAccountId}.{tenantId}-{environment}-accesstoken-{clientId}-{tenantId}", Value = JsonSerializer.Serialize(accessToken, JsonSerializerOptions) },
            new() { Name = $"{localAccountId}.{tenantId}-{environment}-{tenantId}", Value = JsonSerializer.Serialize(accountInfo, JsonSerializerOptions) },
        };
    }

    public class MsalAccountInfo
    {
        public string AuthorityType { get; init; } = "foobar";
        public string ClientInfo { get; init; } = "foobar";
        public string Environment { get; init; } = "login.windows.net";
        public required string HomeAccountId { get; init; }
        public required string LocalAccountId { get; init; }
        public string Name { get; init; } = "foobar";
        public required string Realm { get; init; }
        public required MsalTenantProfile[] TenantProfiles { get; init; }
        public string Username { get; init; } = "foo@bar.com";
    }

    public class MsalTenantProfile
    {
        public bool IsHomeTenant { get; init; } = true;
        public required string LocalAccountId { get; init; }
        public string Name { get; init; } = "foobar";
        public required string TenantId { get; init; }
    }

    public class MsalAccessToken
    {
        public string CachedAt { get; init; } = DateTimeOffset.MinValue.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        public required string ClientId { get; init; }
        public string CredentialType { get; init; } = "AccessToken";
        public string Environment { get; init; } = "login.windows.net";
        public string ExpiresOn { get; init; } = DateTimeOffset.MaxValue.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        public string ExtendedExpiresOn { get; init; } = DateTimeOffset.MaxValue.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        public required string HomeAccountId { get; init; }
        public required string Realm { get; init; }
        public required string Secret { get; init; }
        public required string Target { get; init; }
        public string TokenType { get; init; } = "Bearer";
    }
}
