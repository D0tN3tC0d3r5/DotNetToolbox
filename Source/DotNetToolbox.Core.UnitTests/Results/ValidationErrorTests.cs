namespace DotNetToolbox.Results;

public class ValidationErrorTests
{
    [Fact]
    public void Constructor_WithMessageOnly_CreatesValidationError()
    {
        //Act
        var validationError = new ValidationError("Error message.");

        //Assert
        validationError.Message.Should().Be("Error message.");
        validationError.Source.Should().BeEmpty();
        validationError.Arguments.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithSourceOnly_CreatesValidationError()
    {
        //Act
        var validationError = new ValidationError("Error message for {0}.", "fieldName");

        //Assert
        validationError.Message.Should().Be("Error message for fieldName.");
        validationError.Source.Should().Be("fieldName");
        validationError.Arguments.Should().BeEquivalentTo(new object?[] { "fieldName" });
    }

    [Fact]
    public void Constructor_WithSourceAndData_CreatesValidationError()
    {
        //Act
        var validationError = new ValidationError("Error message for {0} at {1}.", "fieldName", 42);

        //Assert
        validationError.Message.Should().Be("Error message for fieldName at 42.");
        validationError.Source.Should().Be("fieldName");
        validationError.Arguments.Should().BeEquivalentTo(new object?[] { "fieldName", 42 });
    }

    [Fact]
    public void DefaultConstructor_WithInvalidMessageTemplate_ThrowsArgumentException()
    {
        //Act
        var action = () => new ValidationError("   ", "fieldName");

        //Assert
        action.Should().Throw<ArgumentException>().WithMessage("'messageTemplate' cannot be null or whitespace. (Parameter 'messageTemplate')");
    }

    [Fact]
    public void Constructor_WithEmptySource_SetsSourceToEmpty()
    {
        //Act
        var error = new ValidationError("Error message", "   ");

        //Assert
        error.Source.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithMessageOnly_SetsSourceToEmpty()
    {
        //Act
        var error = new ValidationError("Error message");

        //Assert
        error.Source.Should().BeEmpty();
    }

    [Theory]
    [InlineData(false, false, false, false)]
    [InlineData(false, true, false, false)]
    [InlineData(false, false, true, false)]
    [InlineData(false, true, true, false)]
    [InlineData(true, false, false, false)]
    [InlineData(true, true, false, false)]
    [InlineData(true, false, true, false)]
    [InlineData(true, true, true, true)]
    public void Equality_ShouldReturnAsExpected(bool isNotNull, bool hasSameTemplate, bool hasSameSource, bool expectedResult)
    {
        var subject = new ValidationError("Some message for {0}: {1}", "someField", "error");

        var other = isNotNull
            ? hasSameTemplate
                ? hasSameSource
                    ? new("Some message for {0}: {1}", "someField", "error")
                    : new ValidationError("Some message for {0}: {1}", "otherField", "error")
                : hasSameSource
                    ? new("Other message for {0}: {1}", "someField", "error")
                    : new ValidationError("Other message for {0}: {1}", "otherField", "error")
            : default;

        //Act
        var result = subject == other;

        //Assert
        result.Should().Be(expectedResult);
    }
}
