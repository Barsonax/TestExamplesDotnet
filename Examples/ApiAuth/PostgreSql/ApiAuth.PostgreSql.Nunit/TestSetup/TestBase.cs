using Microsoft.Extensions.DependencyInjection;

namespace ApiAuth.PostgreSql.Nunit.TestSetup;

public abstract class TestBase
{
#pragma warning disable NUnit1032
    protected AuthApi Sut { get; private set; } = null!;
#pragma warning restore NUnit1032

    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        Sut = _scope.ServiceProvider.GetRequiredService<AuthApi>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }
}
