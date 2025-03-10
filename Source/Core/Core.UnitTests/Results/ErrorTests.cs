namespace DotNetToolbox.Results;

public class ErrorTests {
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void WithEmptyMessage_Throws(string? message) {
        // Act
        var action = () => new Error(message!);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WithSourceAndMessage_ReturnsFormattedMessage() {
        // Arrange
        var error1 = new Error("Some message.", "Field");

        // Act
        var error2 = error1 with { };

        // Assert
        error2.Should().NotBeSameAs(error1);
        error1.Message.Should().Be("Some message.");
        error1.Sources.Should().BeEquivalentTo("Field");
        error1.ToString().Should().Be("{Message: Some message., Sources: [Field]}");
    }

    [Theory]
    [InlineData("Some message 1.")]
    [InlineData("Some message with 42.")]
    public void FormattedMessage_WithoutSource_ReturnsMessage(string message) {
        // Act
        var error = new Error(message);

        // Assert
        error.Sources.Should().BeEmpty();
        error.Message.Should().Be(message);
    }

    [Theory]
    [InlineData("Error message data", new[] { "field1", "field2" }, true)]
    [InlineData("Other message data", new[] { "field1", "field2" }, false)]
    [InlineData("Error message data", new[] { "field3", "field4" }, false)]
    [InlineData("Error message data", new string[] { }, false)]
    [InlineData("Error message data", new[] { "field1" }, false)]
    [InlineData("Error message data", new[] { "field1", "field2", "field3" }, false)]
    [InlineData("  Error message data ", new[] { "  ", "field1", null, "field2", "" }, true)]
    public void Equality_ShouldReturnAsExpected(string message, string[] sources, bool expectedResult) {
        var subject = new Error("Error message data", "field1", "field2");
        var other = new Error(message, sources);

        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void GetHashCode_ShouldReturnAsExpected() {
        // Arrange & Act
        var errorSet = new HashSet<Error> {
            new("Some message 1.", "Field1"),
            new("Some message 1.", "Field1"),
            new("Some message 1.", "Field1"),
            new(" Some message 1. ", "Field1"),
            new("Some message 1.", " Field1 ", "  "),
            new("Some message 1.", "Field2"),
            new("Some message 2.", "Field1"),
            new("Some message 1.", "Field1"),
        };

        // Assert
        errorSet.Should().BeEquivalentTo(new Error[] {
            new("Some message 1.", "Field1"),
            new("Some message 1.", "Field2"),
            new("Some message 2.", "Field1"),
        });
    }
}
