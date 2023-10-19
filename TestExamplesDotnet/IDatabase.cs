using Microsoft.Extensions.Hosting;

namespace TestExamplesDotnet;

public interface IDatabase
{
    string ConnectionString { get; }

    public void Initialize(IHost host);
    public ValueTask Clean();
}
