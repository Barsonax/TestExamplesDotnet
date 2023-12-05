using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace AngularAuth.Playwright.TestSetup;

public abstract class TestBase : BrowserTest
{
    public IBrowserContext Context { get; private set; } = null!;
    public IPage Page { get; private set; } = null!;
    protected AngularAuthSut Sut { get; private set; } = null!;

    private AsyncServiceScope _scope;

    [SetUp]
    public async Task BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<AngularAuthSut>();

        Context = await NewContext(ContextOptions()).ConfigureAwait(false);
        Page = await Context.NewPageAsync().ConfigureAwait(false);

        Page.Console += (_, e) => TestContext.Out.WriteLine(e.Text);
        Page.PageError += (_, e) => TestContext.Error.WriteLine(e);
    }

    private BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            RecordVideoDir = "videos",
            StorageState = new BrowserStorageState
            {
                Origins = new BrowserStorageStateOrigin[]
                {
                    new()
                    {
                        Origin = Sut.ServerAddress,
                        LocalStorage = MsalLocalStorageTestGenerator.Generate(Array.Empty<Claim>())
                    }
                }
            }.Serialize()
        };
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
