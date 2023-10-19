using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Razor.PostgreSql.Playwright.TestSetup;

public abstract class AwesomeApiTests : PageTest
{
#pragma warning disable NUnit1032
    protected AwesomeApiTestSut Sut { get; private set; } = null!;
#pragma warning restore NUnit1032

    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = AwesomeApiTestSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<AwesomeApiTestSut>();
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
        var folder = Path.GetDirectoryName(path);
        var newName = Path.Combine(folder, $"{TestContext.CurrentContext.Test.FullName}.webm");
        File.Move(path, newName, true);

        await _scope.DisposeAsync();
    }
}
