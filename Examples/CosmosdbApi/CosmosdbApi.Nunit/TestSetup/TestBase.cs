using Microsoft.Extensions.DependencyInjection;

namespace Api.MsSql.Nunit.TestSetup;

public abstract class TestBase
{
    protected ApiMsSqlSut Sut { get; private set; } = null!;

    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<ApiMsSqlSut>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }
}
