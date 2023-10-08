using Microsoft.Extensions.Hosting;

namespace AwesomeApiTest;

public interface IDatabase
{
    string ConnectionString { get; }

    public void Initialize(IHost host);
    public ValueTask Clean();
}