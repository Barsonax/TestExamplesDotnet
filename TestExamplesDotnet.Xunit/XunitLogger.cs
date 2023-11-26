using Microsoft.Extensions.Logging;

namespace TestExamplesDotnet.Xunit;

public sealed class XunitLogger(Context context, string name) : ILogger, IDisposable
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => this;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        context.WriteLine($"[{DateTime.Now}] {logLevel}: {name}[{eventId.Id}]{Environment.NewLine}" +
                         $"{formatter(state, exception)}");
    }

    public void Dispose() { }
}
