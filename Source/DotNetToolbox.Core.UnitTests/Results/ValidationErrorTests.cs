namespace System.Results;

public class ValidationErrorTests {
    [Fact]
    public void DefaultConstructor_WithObjectInitializer_SetsInitializedProperties() {
        //Act
        var validationError = new ValidationError("Error message for {0} at {1}.", "fieldName", 42);

        //Assert
        validationError.Message.Should().Be("Error message for fieldName at 42.");
        validationError.Arguments.Should().BeEquivalentTo(new object?[] { "fieldName", 42 });
    }

    [Fact]
    public void DefaultConstructor_WithInvalidMessageTemplate_ThrowsArgumentException() {
        //Act
        var action = () => new ValidationError("   ", "fieldName");

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage("'messageTemplate' cannot be empty or whitespace. (Parameter 'messageTemplate')");
    }

    [Fact]
    public void Constructor_WithNullField_SetsFieldToNull() {
        //Act
        var action = () => new ValidationError("Error message", "   ");

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage("'source' cannot be empty or whitespace. (Parameter 'source')");
    }

    [Theory]
    [InlineData(true, true, false)]
    [InlineData(false, true, true)]
    [InlineData(false, false, false)]
    public void Equality_ShouldReturnAsExpected(bool isNull, bool sameTemplate, bool expectedResult) {
        var subject = new ValidationError("Error message {0}", "fieldName", "data");
        var same = new ValidationError("Error message {0}", "fieldName", "data");
        var other = new ValidationError("Other message", "fieldName");

        //Act
        var result = subject == (isNull ? default : sameTemplate ? same : other);

        //Assert
        result.Should().Be(expectedResult);
    }
}
