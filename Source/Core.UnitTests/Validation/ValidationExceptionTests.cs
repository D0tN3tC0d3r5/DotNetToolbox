namespace DotNetToolbox.Validation;

public class ValidationExceptionTests {
    [Fact]
    public void ConstructorWithMessageOnly_CreatesException() {
        // Arrange
        var error1 = new ValidationError();

        // Act
        var exception = new ValidationException("Some error.");

        // Assert
        exception.Errors.Should().BeEquivalentTo(new[] { error1 });
        exception.Message.Should().Be("Some error.");
    }

    [Fact]
    public void ConstructorWithSourceAndMessage_CreatesException() {
        // Arrange
        var error1 = new ValidationError();

        // Act
        var exception = new ValidationException("Some error.", "Field1");

        // Assert
        exception.Errors.Should().BeEquivalentTo(new[] { error1 });
        exception.Message.Should().Be("Field1: Some error.");
    }

    [Fact]
    public void ConstructorWithOneError_CreatesException() {
        // Arrange
        var error1 = new ValidationError("Some error 42.");

        // Act
        var exception = new ValidationException(error1);

        // Assert
        exception.Errors.Should().BeEquivalentTo(new[] { error1 });
        exception.Message.Should().Be("Validation failed.");
    }

    [Fact]
    public void ConstructorWithErrorCollection_CreatesException() {
        // Arrange
        var error1 = new ValidationError("Some global error 42.");
        var error2 = new ValidationError("Some other error 13.", "Field1");

        // Act
        var exception = new ValidationException(new[] { error1, error2 });

        // Assert
        exception.Errors.Should().BeEquivalentTo(new[] { error1, error2 });
        exception.Message.Should().Be("Validation failed.");
    }
}
