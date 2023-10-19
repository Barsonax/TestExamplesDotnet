using Microsoft.Extensions.Hosting;

namespace TestExamplesDotnet;

public interface IDatabaseInitializer
{
    void Initialize(IHost app);
}
