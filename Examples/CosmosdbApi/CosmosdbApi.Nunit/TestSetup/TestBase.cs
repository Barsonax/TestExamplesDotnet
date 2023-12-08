using Microsoft.Extensions.DependencyInjection;

namespace CosmosdbApi.Nunit.TestSetup;

public abstract class TestBase
{
    protected CosmosdbApiSut Sut { get; private set; } = null!;

    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<CosmosdbApiSut>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }
}
