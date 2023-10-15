using Microsoft.Extensions.Hosting;

namespace AwesomeApiTest;

public interface IDatabaseInitializer
{
    void Initialize(IHost app);
}
