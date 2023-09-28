using static DotNetToolbox.Results.ValidationResult;

namespace DotNetToolbox.Results;

public class ValidationResultTests
{
    private static readonly ValidationError _error = new("Some {1} for {0}.", "Source", "error");

    private static readonly ValidationResult _success = Success();
    private static readonly ValidationResult _invalid = Failure("Some {1} for {0}.", "Source", "error");
    private static readonly ValidationResult _invalidFromResult = Failure(_invalid);
    private static readonly ValidationResult _invalidFromError = Failure(_error);
    private static readonly ValidationResult _invalidFromErrors = Failure(new[] { _error });
    private static readonly ValidationResult _invalidWithMessageOnly = Failure("Some error.");
    private static readonly ValidationResult _invalidWithSourceOnly = Failure("Some error for {0}.", "Source");
    private static readonly ValidationResult _invalidWithOtherSource = Failure("Some {1} for {0} .", "OtherSource", "error");
    private static readonly ValidationResult _invalidWithOtherData = Failure("Some {1} for {0} .", "Source", "other error");
    private static readonly ValidationResult _invalidWithOtherMessage = Failure("Other {1} for {0} .", "SomeSource", "error");

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure()
    {
        // Act
        ValidationResult result = new ValidationError("Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure()
    {
        // Act
        ValidationResult result = new[] { new ValidationError("Some error.") };

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure()
    {
        // Act
        ValidationResult result = new List<IValidationError> { new ValidationError("Some error.") };

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromDictionary_ReturnsFailure()
    {
        // Act
        ValidationResult result = new Dictionary<string, string[]>() { ["SomeField"] = new[] { "Some error 1.", "Some error 1." } };

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<ValidationResult, bool, bool>
    {
        public TestDataForProperties()
        {
            Add(_invalidWithMessageOnly, true, false);
            Add(_success, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForProperties))]
    public void Properties_ShouldReturnAsExpected(ValidationResult subject, bool isInvalid, bool isSuccess)
    {
        // Assert
        subject.IsFailure.Should().Be(isInvalid);
        subject.IsSuccess.Should().Be(isSuccess);
    }

    private class TestDataForEquality : TheoryData<ValidationResult, ValidationResult?, bool>
    {
        public TestDataForEquality()
        {
            Add(_success, null, false);
            Add(_success, _success, true);
            Add(_success, _invalid, false);
            Add(_invalid, null, false);
            Add(_invalid, _success, false);
            Add(_invalid, _invalid, true);
            Add(_invalid, _invalidFromResult, true);
            Add(_invalid, _invalidFromError, true);
            Add(_invalid, _invalidFromErrors, true);
            Add(_invalid, _invalidWithMessageOnly, false);
            Add(_invalid, _invalidWithSourceOnly, true);
            Add(_invalid, _invalidWithOtherSource, false);
            Add(_invalid, _invalidWithOtherData, false);
            Add(_invalid, _invalidWithOtherMessage, false);
        }
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void Equals_ReturnsAsExpected(ValidationResult subject, ValidationResult? other, bool expectedResult)
    {
        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void NotEquals_ReturnsAsExpected(ValidationResult subject, ValidationResult? other, bool expectedResult)
    {
        // Act
        var result = subject != other;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_DifferentiatesAsExpected()
    {
        var expectedResult = new HashSet<ValidationResult> {
            _success,
            _invalidWithMessageOnly,
            _invalidWithSourceOnly,
            _invalidWithOtherSource,
        };

        // Act
        var result = new HashSet<ValidationResult> {
            Success(),
            Success(),
            _success,
            _success,
            _invalidWithMessageOnly,
            _invalidWithMessageOnly,
            _invalidWithSourceOnly,
            _invalidWithOtherSource,
            _invalidWithSourceOnly,
            _invalidWithOtherSource,
            _success,
            _invalidWithOtherSource,
            _success,
            Success(),
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void AddOperator_FromSuccess_WithFailed_ReturnsInvalid()
    {
        // Arrange
        var result = Success();
        var other = Failure("Some error.");

        // Act
        result += other;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().ContainSingle();
    }

    [Fact]
    public void AddOperator_FromSuccess_WithSuccess_ReturnsSuccess()
    {
        // Arrange
        var result = Success();
        var other = Success();

        // Act
        result += other;

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void AddOperator_FromSuccess_WithError_ReturnsInvalid()
    {
        // Arrange
        var result = Success();

        // Act
        result += new ValidationError("Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().ContainSingle();
    }

    [Fact]
    public void AddOperator_FromFailed_WithFailed_ReturnsInvalidWithDistinctErrors()
    {
        // Arrange
        var result = Failure("Some error 1.") + Failure("Some error 2.") + Failure("Some error 3.");
        var other = Failure("Some error 2.") + new ValidationError[] { new("Some error 3."), new("Some error 4.") };

        // Act
        result += other;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().BeEquivalentTo(new[]
        {
            new ValidationError("Some error 1."),
            new ValidationError("Some error 2."),
            new ValidationError("Some error 3."),
            new ValidationError("Some error 4."),
        });
        result.ValidationErrors.Should().HaveCount(4);
    }

    [Fact]
    public void AddOperator_FromFailed_WithSuccess_ReturnsSuccess()
    {
        // Arrange
        var result = Failure("Some error.");
        var other = Success();

        // Act
        result += other;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().ContainSingle();
    }

    [Fact]
    public void AddOperator_FromFailed_WithError_ReturnsInvalid()
    {
        // Arrange
        var result = Failure("Some error.");

        // Act
        result += new ValidationError("Some other error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().HaveCount(2);
    }

    [Fact]
    public void AddOperator_FromFailed_WithSameError_ReturnsInvalid()
    {
        // Arrange
        var result = Failure("Some error.");

        // Act
        result += new ValidationError("Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().ContainSingle();
    }
}
