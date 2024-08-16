namespace DotNetToolbox.Results;

public class ValidationErrorTests {
    [Fact]
    public void DefaultConstructor_CreatesNewError() {
        // Arrange
        var error1 = new ValidationError();

        // Act
        var error2 = error1 with { };

        // Assert
        error2.Should().NotBeSameAs(error1);
        error1.Message.Should().Be(ValidationError.DefaultErrorMessage);
        error1.Source.Should().BeEmpty();
        error1.Describe().Should().Be("The value is invalid.");
    }

    [Fact]
    public void WithSourceAndMessage_ReturnsFormattedMessage() {
        // Arrange
        var error1 = new ValidationError("Some message.", "Field");

        // Act
        var error2 = error1 with { };

        // Assert
        error2.Should().NotBeSameAs(error1);
        error1.Message.Should().Be("Some message.");
        error1.Source.Should().Be("Field");
        error1.Describe().Should().Be("Field: Some message.");
    }

    [Theory]
    [InlineData("Some message 1.")]
    [InlineData("Some message with 42.")]
    public void FormattedMessage_WithoutSource_ReturnsMessage(string message) {
        // Act
        var error = new ValidationError(message);

        // Assert
        error.Source.Should().Be(string.Empty);
        error.Message.Should().Be(message);
    }

    [Fact]
    public void Equality_ShouldReturnAsExpected() {
        var subject = new ValidationError("Break message data", "field");
        var same = new ValidationError("Break message data", "field");
        var otherSource = new ValidationError("Break message data", "otherField");
        var otherMessage = new ValidationError("Other message data", "field");

        //Act
        var resultForNull = subject == null!;
        var resultForOtherSource = subject != otherSource;
        var resultForOtherTemplate = subject != otherMessage;
        var resultForSame = subject == same;

        //Assert
        resultForNull.Should().BeFalse();
        resultForOtherSource.Should().BeTrue();
        resultForOtherTemplate.Should().BeTrue();
        resultForSame.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ShouldReturnAsExpected() {
        // Arrange & Act
        var errorSet = new HashSet<ValidationError> {
            new("Source 1", "Some message 1 42."),
            new("Source 1", "Some message 1 42."),
            new(" Source 1 ", "Some message 1 42."),
            new("Source 1", "Some message 1 42."),
            new("Source 2", "Some message 1 42."),
            new("Source 1", "Some message 2 42."),
            new("Source 1", "Some message 1 7."),
        };

        // Assert
        errorSet.Should().BeEquivalentTo(new ValidationError[] {
            new("Source 1", "Some message 1 42."),
            new("Source 2", "Some message 1 42."),
            new("Source 1", "Some message 2 42."),
            new("Source 1", "Some message 1 7."),
        });
    }
}
