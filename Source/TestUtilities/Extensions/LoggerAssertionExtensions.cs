namespace DotNetToolbox.Core.TestUtilities.Extensions;

public static class LoggerAssertionExtensions {
    public static ILoggerFactory CreateFactory(this ILogger logger) {
        var loggerFactory = Substitute.For<ILoggerFactory>();
        logger.IsEnabled(Arg.Any<LogLevel>()).Returns(true);
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);
        return loggerFactory;
    }

    public static void ShouldContain<TType>(this ILogger<TType> logger, LogLevel level, string message, EventId eventId = default)
        => logger.ReceivedCalls().Should().Contain(c => c.Has(level, message, eventId));

    private static bool Has(this ICall call, LogLevel level, string message, EventId eventId)
        => call.GetArguments() is [LogLevel ll, EventId evt, string msg, ..,]
        && ll == level
        && evt.Id == eventId.Id
        && msg == message;
}
