using Microsoft.Extensions.DependencyInjection;

namespace Api.PostgreSql.Nunit.TestSetup;

public abstract class TestBase
{
    protected ApiPostgreSqlSut Sut { get; private set; } = null!;

    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<ApiPostgreSqlSut>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }
}
