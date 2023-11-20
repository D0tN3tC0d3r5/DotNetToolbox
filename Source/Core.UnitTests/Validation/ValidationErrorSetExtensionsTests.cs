namespace DotNetToolbox.Validation;

public class ValidationErrorSetExtensionsTests {
    [Theory]
    [InlineData("Field1: Some message 1.", true)]
    [InlineData("Field1: Some message 2.", true)]
    [InlineData("Field2: Some message 42.", true)]
    [InlineData("Field2: Some message 1.", false)]
    [InlineData("Field1: Some message 42.", false)]
    public void Contains_ReturnsIfHasFormattedMessage(string message, bool expectedResult) {
        // Arrange
        var errors = new List<ValidationError> {
            new("Field1", "Some message 1."),
            new("Field1", "Some message 2."),
            new("Field2", "Some message {0}.", 42),
        };

        // Act & Assert
        errors.Contains(message).Should().Be(expectedResult);
    }
}
