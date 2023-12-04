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
        return new BrowserNewContextOptions { RecordVideoDir = "videos", StorageState = JsonSerializer.Serialize(storageState) };
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
