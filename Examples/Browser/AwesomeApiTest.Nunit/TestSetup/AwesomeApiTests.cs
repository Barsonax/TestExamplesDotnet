using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright.NUnit;

namespace AwesomeApiTest.Nunit.TestSetup;

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

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }
}
