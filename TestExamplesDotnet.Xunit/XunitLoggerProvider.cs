using Microsoft.Extensions.Logging;

namespace TestExamplesDotnet.Xunit;

public sealed class XunitLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new XunitLogger(XunitContext.Context, categoryName);
    }

    public void Dispose()
    {
    }
}
