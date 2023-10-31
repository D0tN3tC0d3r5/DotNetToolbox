namespace System.Validation;

public class ValidationExceptionTests {
    [Fact]
    public void ConstructorWithMessageOnly_CreatesException() {
        // Arrange
        var error1 = new ValidationError("Some error.");

        // Act
        var exception = new ValidationException("Some error.");

        // Assert
        exception.Errors.Should().BeEquivalentTo(new[] { error1 });
        exception.Message.Should().Be("Validation failed.");
    }

    [Fact]
    public void ConstructorWithSourceAndMessage_CreatesException() {
        // Arrange
        var error1 = new ValidationError("Field1", "Some error.");

        // Act
        var exception = new ValidationException("Field1", "Some error.");

        // Assert
        exception.Errors.Should().BeEquivalentTo(new[] { error1 });
        exception.Message.Should().Be("Validation failed.");
    }

    [Fact]
    public void ConstructorWithOneError_CreatesException() {
        // Arrange
        var error1 = new ValidationError("Some error {0}.", 42);

        // Act
        var exception = new ValidationException(error1);

        // Assert
        exception.Errors.Should().BeEquivalentTo(new[] { error1 });
        exception.Message.Should().Be("Validation failed.");
    }

    [Fact]
    public void ConstructorWithErrorCollection_CreatesException() {
        // Arrange
        var error1 = new ValidationError("Some global error {0}.", 42);
        var error2 = new ValidationError("Field1", "Some other error {0}.", 13);

        // Act
        var exception = new ValidationException(new[] { error1, error2 });

        // Assert
        exception.Errors.Should().BeEquivalentTo(new[] { error1, error2 });
        exception.Message.Should().Be("Validation failed.");
    }
}
