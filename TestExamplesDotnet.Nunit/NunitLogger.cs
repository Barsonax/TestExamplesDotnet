using Microsoft.Extensions.Logging;

namespace TestExamplesDotnet.Nunit;

public sealed class NunitLogger(TextWriter output) : ILogger, IDisposable
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => this;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        output.WriteLine(formatter(state, exception));
    }

    public void Dispose() { }
}
