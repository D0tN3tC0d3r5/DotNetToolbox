namespace DotNetToolbox.Core.TestUtilities.Extensions;

public static class LoggerAssertionExtensions {
    public static ILoggerFactory CreateFactory(this ILogger logger) {
        var loggerFactory = Substitute.For<ILoggerFactory>();
        logger.IsEnabled(Arg.Any<LogLevel>()).Returns(true);
        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);
        return loggerFactory;
    }

    public static void ShouldContain<TType>(this ILogger<TType> logger, LogLevel level, string message, EventId eventId = default) {
        var calls = logger.ReceivedCalls().ToArray();
        calls.Should().Contain(l => Contains(l, level, message, eventId));
    }

    private static bool Contains(ICall call, LogLevel level, string message, EventId eventId) {
        var arguments = call.GetArguments();
        return arguments.Length == 5
               && arguments[0] is LogLevel ll && ll == level
               && arguments[1] is EventId evt && evt == eventId
               && arguments[2]?.ToString() == message;
    }
}
