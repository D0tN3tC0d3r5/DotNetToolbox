namespace System.Validation;

public class ValidationErrorTests {
    [Theory]
    [InlineData("Source1", "Some message 1.", null!, "Source1: Some message 1.")]
    [InlineData("Source1", "Some message 1.", new object[] { }, "Source1: Some message 1.")]
    [InlineData("Source1", "Some message with {0}.", new object[] { 42 }, "Source1: Some message with 42.")]
    [InlineData(null, "Some message with {0}.", new object[] { 42 }, "Some message with 42.")]
    [InlineData("", "Some message with {0}.", new object[] { 42 }, "Some message with 42.")]
    [InlineData("   ", "Some message with {0}.", new object[] { 42 }, "Some message with 42.")]
    public void FormattedMessage_WithSource_ReturnsMessage(string source, string template, object[] args, string expectedMessage) {
        // Act
        var error = new ValidationError(source, template, args);

        // Assert
        error.Source.Should().Be(source ?? string.Empty);
        error.MessageTemplate.Should().Be(template);
        error.Arguments.Should().BeEquivalentTo(args ?? Array.Empty<object>());
        error.FormattedMessage.Should().Be(expectedMessage);
    }

    [Theory]
    [InlineData("Some message 1.", null!, "Some message 1.")]
    [InlineData("Some message with {0}.", new object[] { 42 }, "Some message with 42.")]
    public void FormattedMessage_WithoutSource_ReturnsMessage(string template, object[] args, string expectedMessage) {
        // Act
        var error = new ValidationError(template, args);

        // Assert
        error.Source.Should().Be(string.Empty);
        error.MessageTemplate.Should().Be(template);
        error.Arguments.Should().BeEquivalentTo(args ?? Array.Empty<object>());
        error.FormattedMessage.Should().Be(expectedMessage);
    }

    [Theory]
    [InlineData(true, true, false)]
    [InlineData(false, true, true)]
    [InlineData(false, false, false)]
    public void Equality_ShouldReturnAsExpected(bool isNull, bool sameTemplate, bool expectedResult) {
        var subject = new ValidationError("fieldName", "Error message {0}", "data");
        var same = new ValidationError("fieldName", "Error message {0}", "data");
        var other = new ValidationError("fieldName", "Other message");

        //Act
        var result = subject == (isNull ? default : sameTemplate ? same : other);

        //Assert
        result.Should().Be(expectedResult);
    }
}
