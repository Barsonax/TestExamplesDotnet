using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Migrations.MsSql.EntityFrameworkCore.Sut;
using Testcontainers.MsSql;
using TestExamplesDotnet;
using TestExamplesDotnet.Mssql;

namespace Migrations.MsSql.EntityFrameworkCore.Nunit;

[FixtureLifeCycle(LifeCycle.SingleInstance)]
[Parallelizable(ParallelScope.Self)]
public class MigrationTests
{
    private MsSqlContainer _databaseContainer = null!;
    private ILogger<MigrationTests> _logger = null!;
    private AsyncServiceScope _scope;

    [SetUp]
    public void BeforeTestCase()
    {
        _scope = GlobalSetup.Provider.CreateAsyncScope();
        _databaseContainer = _scope.ServiceProvider.GetRequiredService<MsSqlContainer>();
        _logger = _scope.ServiceProvider.GetRequiredService<ILogger<MigrationTests>>();
    }

    [TearDown]
    public async Task AfterTestCase()
    {
        await _scope.DisposeAsync();
    }

    [TestCaseSource(typeof(MigrationTestCases))]
    public async Task MigrationsUpAndDown_NoErrors2(MigrationScript migration)
    {
        var databaseName = "MigrationsTest";
        await _databaseContainer.CreateDatabase(databaseName);
        var migrator = new SqlMigrator(_databaseContainer, _logger, databaseName);
        var upResult = await migrator.Up(migration);
        upResult.ExitCode.Should().Be(0, $"Error during migration up: {upResult.Stderr}");
        var downResult = await migrator.Down(migration);
        downResult.ExitCode.Should().Be(0, $"Error during migration down: {downResult.Stderr}");
        var upResult2 = await migrator.Up(migration);
        upResult.ExitCode.Should().Be(0, $"Error during migration up2: {upResult2.Stderr}");
    }

    private sealed class MigrationTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            using DbContext context = new BloggingContext();
            var migrations = context.GenerateMigrationScripts();

            foreach (var migration in migrations)
            {
                yield return new TestCaseData(migration);
            }
        }
    }
}
