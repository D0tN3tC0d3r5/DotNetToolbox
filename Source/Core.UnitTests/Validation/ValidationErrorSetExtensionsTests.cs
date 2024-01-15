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
            new("Some message 1.", "Field1"),
            new("Some message 2.", "Field1"),
            new("Some message 42.", "Field2"),
        };

        // Act & Assert
        errors.Contains(message).Should().Be(expectedResult);
    }
}
