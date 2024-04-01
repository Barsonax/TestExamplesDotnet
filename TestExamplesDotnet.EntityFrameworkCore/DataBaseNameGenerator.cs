using Microsoft.EntityFrameworkCore;

namespace TestExamplesDotnet;

public sealed class DataBaseNameGenerator<TDbContext> : IDataBaseNameGenerator
    where TDbContext : DbContext
{
    private int _counter;

    public string GetDataBaseName()
    {
        var migrationId = typeof(TDbContext).Assembly.ManifestModule.ModuleVersionId.ToString("N");;

        var counter = Interlocked.Increment(ref _counter);
        return $"{migrationId}_{counter}";
    }
}
