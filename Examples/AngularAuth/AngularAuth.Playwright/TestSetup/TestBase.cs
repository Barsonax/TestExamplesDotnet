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

        Page.PageError += (_, e) => TestContext.Error.WriteLine(e);
    }

    private BrowserNewContextOptions ContextOptions()
    {
        var storageState = new {
            cookies = new string[] { },
            origins = new object[]
            {
                new
                {
                    origin = Sut.ServerAddress,
                    localStorage = new object[]
                    {
                        new
                        {
                            name = "foobar",
                            value = "123"
                        }
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


public class BrowserStorageState
{
    public BrowserStorageStateCookie[] Cookies { get; init; } = Array.Empty<BrowserStorageStateCookie>();
    public BrowserStorageStateOrigin[] Origins { get; init; } = Array.Empty<BrowserStorageStateOrigin>();
}

public record BrowserStorageStateCookie(string Name, string Value, string Domain, string Path, float Expires,  bool HttpOnly, bool Secure, string SameSite);
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
