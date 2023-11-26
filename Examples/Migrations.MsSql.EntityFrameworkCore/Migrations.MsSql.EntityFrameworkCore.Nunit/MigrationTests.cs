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
        await migrator.Up(migration);
        await migrator.Down(migration);
        await migrator.Up(migration);
    }

    private class MigrationTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            using DbContext context = new BloggingContext(new DbContextOptionsBuilder<BloggingContext>().UseSqlServer().Options);
            var migrations = context.GenerateMigrationScripts();

            foreach (var migration in migrations)
            {
                yield return new TestCaseData(migration);
            }
        }
    }
}
