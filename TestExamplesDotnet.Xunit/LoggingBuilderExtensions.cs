using Microsoft.Extensions.Logging;

namespace TestExamplesDotnet.Xunit;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddXunitLogging(this ILoggingBuilder services)
    {
#pragma warning disable CA2000
        services.AddProvider(new XunitLoggerProvider());
#pragma warning restore CA2000
        return services;
    }
}
