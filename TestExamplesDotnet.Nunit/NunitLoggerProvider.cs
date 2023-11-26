using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace TestExamplesDotnet.Nunit;

public sealed class NunitLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new NunitLogger(TestContext.Out, categoryName);
    }

    public void Dispose()
    {
    }
}
