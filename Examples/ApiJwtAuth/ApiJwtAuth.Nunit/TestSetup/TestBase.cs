using Microsoft.Extensions.DependencyInjection;

namespace ApiJwtAuth.Nunit.TestSetup;

public abstract class TestBase
{
    protected ApiJwtAuthSut Sut { get; private set; } = null!;

    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<ApiJwtAuthSut>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }
}
