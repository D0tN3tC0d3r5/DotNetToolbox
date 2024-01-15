namespace DotNetToolbox.Validation;

public class ValidationErrorTests {
    [Fact]
    public void DefaultConstructor_CreatesNewError() {
        // Arrange
        var error1 = new ValidationError();

        // Act
        var error2 = error1;

        // Assert
        error2.Should().BeSameAs(error1);
        error1.Message.Should().Be(ValidationError.DefaultErrorMessage);
        error1.Source.Should().BeEmpty();
    }

    [Fact]
    public void FormattedMessage_WithNullError_ReturnsEmptyMessage() {
        // Act
        var error = default(ValidationError);

        // Assert
        error.Should().BeNull();
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
        var otherSource = new ValidationError("Break message data", "otherField");
        var otherTemplate = new ValidationError("Other message data", "field");
        var otherData = new ValidationError("Break message other data", "field");
        var same = new ValidationError("Break message data", "field");

        //Act
        var resultForOtherSource = subject != otherSource;
        var resultForOtherTemplate = subject != otherTemplate;
        var resultForOtherData = subject != otherData;
        var resultForSame = subject == same;

        //Assert
        resultForOtherSource.Should().BeTrue();
        resultForOtherTemplate.Should().BeTrue();
        resultForOtherData.Should().BeTrue();
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
