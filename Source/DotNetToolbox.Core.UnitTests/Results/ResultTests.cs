using static System.Results.Result;

namespace System.Results;

public class ResultTests {
    private static readonly Result _success = Success();
    private static readonly Result _invalid = Invalid("Some error.", "Source");
    private static readonly Result _invalidWithSameError = Invalid("Some error.", "Source");
    private static readonly Result _invalidWithOtherError = Invalid("Other error.", "Source");

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure() {
        // Act
        Result result = new ValidationError("Some error.", nameof(result));

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure() {
        // Act
        Result result = new[] { new ValidationError("Some error.", nameof(result)) };

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure() {
        // Act
        Result result = new List<ValidationError> { new("Some error.", nameof(result)) };

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<Result, bool, bool> {
        public TestDataForProperties() {
            Add(_invalid, true, false);
            Add(_success, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForProperties))]
    public void Properties_ShouldReturnAsExpected(Result subject, bool isInvalid, bool isSuccess) {
        // Assert
        subject.IsInvalid.Should().Be(isInvalid);
        subject.IsSuccess.Should().Be(isSuccess);
    }

    private class TestDataForEquality : TheoryData<Result, Result?, bool> {
        public TestDataForEquality() {
            Add(_success, null, false);
            Add(_success, _success, true);
            Add(_success, _invalid, false);
            Add(_invalid, null, false);
            Add(_invalid, _success, false);
            Add(_invalid, _invalid, true);
            Add(_invalid, _invalidWithSameError, true);
            Add(_invalid, _invalidWithOtherError, false);
        }
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void Equals_ReturnsAsExpected(Result subject, Result? other, bool expectedResult) {
        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void NotEquals_ReturnsAsExpected(Result subject, Result? other, bool expectedResult) {
        // Act
        var result = subject != other;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_DifferentiatesAsExpected() {
        var expectedResult = new HashSet<Result> {
            _success,
            _invalid,
            _invalidWithOtherError
        };

        // Act
        var result = new HashSet<Result> {
            Success(),
            Success(),
            _success,
            _success,
            _invalid,
            _invalid,
            _invalidWithSameError,
            _invalidWithOtherError,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void AddOperator_WithError_ReturnsInvalid() {
        // Arrange
        var result = Success();

        // Act
        result += new ValidationError("Some error.", "result");

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValue_ReturnsSuccess() {
        // Act
        Result<string> subject = "Value";

        // Assert
        subject.Value.Should().Be("Value");
        subject.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void AddOperator_WithValueAndWithoutError_ReturnsInvalid() {
        // Arrange
        var result = Success("Value");

        // Act
        result += Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsInvalid.Should().BeFalse();
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void AddOperator_WithValueAndWithError_ReturnsInvalid() {
        // Arrange
        var result = Success("Value");

        // Act
        result += new ValidationError("Some error.", "result");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsInvalid.Should().BeTrue();
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void MapTo_WithoutError_ReturnsSuccess() {
        // Arrange
        var subject = Success("42");

        // Act
        var result = subject.MapTo(int.Parse);

        // Assert
        result.Should().BeOfType<Result<int>>();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void MapTo_WithError_ReturnsInvalid() {
        // Arrange
        var subject = Invalid("42", "Some error.", "Field");

        // Act
        var result = subject.MapTo(int.Parse);

        // Assert
        result.Should().BeOfType<Result<int>>();
        result.IsSuccess.Should().BeFalse();
    }
}
