using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace VueAuth.Playwright.TestSetup;

public abstract class TestBase : PageTest
{
    protected VueSut Sut { get; private set; } = null!;

    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<VueSut>();
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
        await Page.Context.CloseAsync();
        var path = await Page.Video!.PathAsync();
        var folder = Path.GetDirectoryName(path) ?? throw new InvalidOperationException($"Could not get folder for {path}");
        var newName = Path.Combine(folder, $"{TestContext.CurrentContext.Test.FullName}.webm");
        File.Move(path, newName, true);

        await _scope.DisposeAsync();
    }
}
