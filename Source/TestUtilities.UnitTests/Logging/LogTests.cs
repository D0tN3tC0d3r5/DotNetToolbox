namespace DotNetToolbox.TestUtilities.Logging;

public class LogTests {
    [Fact]
    public void Log_Constructor_ShouldSetPropertiesCorrectly() {
        // Arrange
        var log = new Log();
        const LogLevel level = LogLevel.Information;
        var eventId = new EventId(1, "TestEvent");
        var state = new { Message = "TestMessage" };
        var exception = new Exception("TestException");
        var message = "TestMessage";

        // Act
        var subject = log with {
            Level = level,
            EventId = eventId,
            State = state,
            Exception = exception,
            Message = message,
        };

        // Assert
        subject.Level.Should().Be(level);
        subject.EventId.Should().Be(eventId);
        subject.State.Should().Be(state);
        subject.Exception.Should().Be(exception);
        subject.Message.Should().Be(message);
    }
}
