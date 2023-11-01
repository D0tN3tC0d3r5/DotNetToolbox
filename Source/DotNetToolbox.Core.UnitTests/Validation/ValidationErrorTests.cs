namespace System.Validation;

public class ValidationErrorTests {
    [Fact]
    public void DefaultConstructor_CreatesNewError() {
        // Arrange
        var error1 = new ValidationError() {
            MessageTemplate = "Some error {0}.",
            Arguments = new object[] { 42 },
        };

        // Act
        var error2 = error1;

        // Assert
        _ = error2.Should().NotBeSameAs(error1);
        _ = error2.FormattedMessage.Should().Be("Some error 42.");
    }

    [Theory]
    [InlineData("Source1", "Some message 1.", new object[] { }, "Source1: Some message 1.")]
    [InlineData("Source1", "Some message with {0}.", new object[] { 42 }, "Source1: Some message with 42.")]
    [InlineData("", " Some message with {0}. ", new object[] { 42 }, " Some message with 42. ")]
    [InlineData("   ", "Some message with {0}.", new object[] { 42 }, "Some message with 42.")]
    public void FormattedMessage_WithSource_ReturnsMessage(string source, string template, object[] args, string expectedMessage) {
        // Act
        var error = new ValidationError(source, template, args);

        // Assert
        _ = error.Source.Should().Be(source.Trim());
        _ = error.MessageTemplate.Should().Be(template);
        _ = error.Arguments.Should().BeEquivalentTo(args);
        _ = error.FormattedMessage.Should().Be(expectedMessage);
    }

    [Fact]
    public void FormattedMessage_WithNullError_ReturnsEmptyMessage() {
        // Act
        var error = default(ValidationError);

        // Assert
        _ = error.Source.Should().Be(string.Empty);
        _ = error.MessageTemplate.Should().Be(string.Empty);
        _ = error.Arguments.Should().BeEmpty();
        _ = error.FormattedMessage.Should().Be(string.Empty);
    }

    [Theory]
    [InlineData("Some message 1.", new object[] { }, "Some message 1.")]
    [InlineData("Some message with {0}.", new object[] { 42 }, "Some message with 42.")]
    public void FormattedMessage_WithoutSource_ReturnsMessage(string template, object[] args, string expectedMessage) {
        // Act
        var error = new ValidationError(template, args);

        // Assert
        _ = error.Source.Should().Be(string.Empty);
        _ = error.MessageTemplate.Should().Be(template);
        _ = error.Arguments.Should().BeEquivalentTo(args);
        _ = error.FormattedMessage.Should().Be(expectedMessage);
    }

    [Fact]
    public void Equality_ShouldReturnAsExpected() {
        var subject = new ValidationError("field", "Error message {0}", "data");
        var otherSource = new ValidationError("otherField", "Error message {0}", "data");
        var otherTemplate = new ValidationError("field", "Other message {0}", "data");
        var otherData = new ValidationError("field", "Error message {0}", "otherData");
        var same = new ValidationError("field", "Error message {0}", "data");

        //Act
        var resultForDefault = subject != default;
        var resultForOtherSource = subject != otherSource;
        var resultForOtherTemplate = subject != otherTemplate;
        var resultForOtherData = subject != otherData;
        var resultForSame = subject == same;

        //Assert
        _ = resultForDefault.Should().BeTrue();
        _ = resultForOtherSource.Should().BeTrue();
        _ = resultForOtherTemplate.Should().BeTrue();
        _ = resultForOtherData.Should().BeTrue();
        _ = resultForSame.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_ShouldReturnAsExpected() {
        // Arrange & Act
        var errorSet = new HashSet<ValidationError> {
            new("Source 1", "Some message 1 {0}.", 42),
            new("Source 1", "Some message 1 {0}.", 42),
            new(" Source 1 ", "Some message 1 {0}.", 42),
            new("Source 1", "Some message 1 {0}.", 42, 13),
            new("Source 2", "Some message 1 {0}.", 42),
            new("Source 1", "Some message 2 {0}.", 42),
            new("Source 1", "Some message 1 {0}.", 7),
        };

        // Assert
        _ = errorSet.Should().BeEquivalentTo(new ValidationError[] {
            new("Source 1", "Some message 1 {0}.", 42),
            new("Source 2", "Some message 1 {0}.", 42),
            new("Source 1", "Some message 2 {0}.", 42),
            new("Source 1", "Some message 1 {0}.", 7),
        });
    }
}
