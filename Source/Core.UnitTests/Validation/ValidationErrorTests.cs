namespace DotNetToolbox.Validation;

public class ValidationErrorTests {
    [Fact]
    public void DefaultConstructor_CreatesNewError() {
        // Arrange
        var error1 = new ValidationError() {
            Message = "Some error 42.",
        };

        // Act
        var error2 = error1;

        // Assert
        error2.Should().NotBeSameAs(error1);
        error2.Message.Should().Be("Some error 42.");
    }

    [Fact]
    public void FormattedMessage_WithNullError_ReturnsEmptyMessage() {
        // Act
        var error = default(ValidationError);

        // Assert
        error.Source.Should().Be(string.Empty);
        error.Message.Should().Be(string.Empty);
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
        var subject = new ValidationError("field", "Break message data");
        var otherSource = new ValidationError("otherField", "Break message data");
        var otherTemplate = new ValidationError("field", "Other message data");
        var otherData = new ValidationError("field", "Break message other data");
        var same = new ValidationError("field", "Break message data");

        //Act
        var resultForDefault = subject != default;
        var resultForOtherSource = subject != otherSource;
        var resultForOtherTemplate = subject != otherTemplate;
        var resultForOtherData = subject != otherData;
        var resultForSame = subject == same;

        //Assert
        resultForDefault.Should().BeTrue();
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
