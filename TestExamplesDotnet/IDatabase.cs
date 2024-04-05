using Microsoft.Extensions.Hosting;

namespace TestExamplesDotnet;

public interface IDatabase
{
    string ConnectionString { get; }

    public void EnsureInitialized(IHost host);
    public Task Clean();
}
