namespace System.Constants;

public class MessagesTests {
    [Theory]
    [InlineData("'Value' cannot be null.", "'Value' must be null.")]
    [InlineData("'Value' must be null.", "'Value' cannot be null.")]
    [InlineData("'Value' is null.", "'Value' is not null.")]
    [InlineData("'Value' is not null.", "'Value' is null.")]
    [InlineData("'Value' something different.", "'Value' something different.")]
    public void NegateMessage_ReturnsNegatedMessage(string message, string expectedMessage) {
        // Act
        var result = NegateMessage(message);

        // Assert
        _ = result.Should().Be(expectedMessage);
    }
}
