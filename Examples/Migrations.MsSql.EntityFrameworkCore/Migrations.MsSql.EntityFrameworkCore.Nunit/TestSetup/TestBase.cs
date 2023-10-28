using Microsoft.Extensions.DependencyInjection;

namespace Migrations.MsSql.EntityFrameworkCore.Nunit.TestSetup;

public abstract class TestBase
{
#pragma warning disable NUnit1032
    protected ApiMsSqlSut Sut { get; private set; } = null!;
#pragma warning restore NUnit1032

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
