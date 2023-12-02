using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Vue.Playwright.TestSetup;

public abstract class TestBase : PageTest
{
    protected VueSut Sut { get; private set; } = null!;

    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<VueSut>();
        Page.PageError += (_, e) => TestContext.Error.WriteLine(e);
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            RecordVideoDir = "videos",
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
