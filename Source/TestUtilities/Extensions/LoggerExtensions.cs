using FluentAssertions.Collections;

namespace DotNetToolbox.TestUtilities.Extensions;

public static class LoggerExtensions {
    public static ILoggerFactory CreateFactory(this ILogger logger) {
        var loggerFactory = Substitute.For<ILoggerFactory>();
        logger.IsEnabled(Arg.Any<LogLevel>())
              .Returns(true);
        loggerFactory.CreateLogger(Arg.Any<string>())
                     .Returns(logger);
        return loggerFactory;
    }

    public static LoggerAssertions Should<TType>(this ILogger<TType> logger) => new(logger.ReceivedCalls());
}
