using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AngularAuth.Playwright.TestSetup;

public abstract class TestBase : BrowserTest
{
    public IBrowserContext Context { get; private set; } = null!;
    public IPage Page { get; private set; } = null!;
    protected VueSut Sut { get; private set; } = null!;

    private AsyncServiceScope _scope;

    [SetUp]
    public async Task BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<VueSut>();

        Context = await NewContext(ContextOptions()).ConfigureAwait(false);
        Page = await Context.NewPageAsync().ConfigureAwait(false);

        Page.Console += (_, e) => TestContext.Out.WriteLine(e.Text);
        Page.PageError += (_, e) => TestContext.Error.WriteLine(e);
    }

    private static JsonSerializerOptions JsonSerializerOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
    private BrowserNewContextOptions ContextOptions()
    {
        var environment = "login.windows.net";
        var clientId = "foobar";
        var localAccountId = Guid.NewGuid().ToString();
        var tenantId = Guid.NewGuid().ToString();
        var homeAccountId = $"{localAccountId}.{tenantId}";
        var jwtToken = TestJwtGenerator.GenerateJwtToken(Array.Empty<Claim>());

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

        var storageState = new BrowserStorageState {
            Origins = new BrowserStorageStateOrigin[]
            {
                new()
                {
                    Origin = Sut.ServerAddress,
                    LocalStorage = new BrowserStorageStateLocalStorage[]
                    {
                        new()
                        {
                            Name = $"{localAccountId}.{tenantId}-{environment}-accesstoken-{clientId}-{tenantId}",
                            Value = JsonSerializer.Serialize(accessToken, JsonSerializerOptions)
                        },
                        new()
                        {
                            Name = $"{localAccountId}.{tenantId}-{environment}-{tenantId}",
                            Value = JsonSerializer.Serialize(accountInfo, JsonSerializerOptions)
                        },
                    }
                }
            }
        };
        var serializedStorageState = JsonSerializer.Serialize(storageState, JsonSerializerOptions);
        return new BrowserNewContextOptions { RecordVideoDir = "videos", StorageState = serializedStorageState};
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await Page.CloseAsync();
        var path = await Page.Video!.PathAsync();
        var folder = Path.GetDirectoryName(path) ?? throw new InvalidOperationException($"Could not get folder for {path}");
        var newName = Path.Combine(folder, $"{TestContext.CurrentContext.Test.FullName}.webm");

        await _scope.DisposeAsync();
        await WaitForFileToBeFreed(path);
        File.Move(path, newName, true);
    }

    private static async Task WaitForFileToBeFreed(string filename)
    {
        while (true)
        {
            try
            {
                await using FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                return;
            }
            catch (IOException) { }
            await Task.Delay(200);
        }
    }
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
    public required  string HomeAccountId { get; init; }
    public required string Realm { get; init; }
    public required string Secret { get; init; }
    public required string Target { get; init; }
    public string TokenType { get; init; } = "Bearer";
}

public class BrowserStorageState
{
    public BrowserStorageStateOrigin[] Origins { get; init; } = Array.Empty<BrowserStorageStateOrigin>();
}

public class BrowserStorageStateOrigin
{
    public required string Origin { get; init; }
    public required BrowserStorageStateLocalStorage[] LocalStorage { get; init; }
}
public class BrowserStorageStateLocalStorage
{
    public required string Name { get; init; }
    public required string Value { get; init; }
}
