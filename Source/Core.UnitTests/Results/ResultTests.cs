namespace DotNetToolbox.Results;

public class ResultTests {
    private static readonly Result _success = Result.Success();
    private static readonly Result _invalid = Result.Invalid("Some error.");
    private static readonly Result _invalidWithSameError = Result.Invalid("Some error.");
    private static readonly Result _invalidWithOtherError = Result.Invalid("Other error.");
    private static readonly Result _failure = Result.Exception("Some error.");

    private static readonly Result<string> _successWithValue = Result.Success("42");
    private static readonly Result<string> _invalidWithValue = Result.Invalid<string>("42", "Some error.");
    private static readonly Result<string> _failureWithValue = Result.Exception("42", "Some error.");

    [Fact]
    public void CopyConstructor_ClonesObject() {
        // Act
        var result = _success with {
            Errors = new HashSet<ValidationError> { new("Some error.") },
        };

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Exception.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure() {
        // Act
        Result result = new ValidationError(nameof(result), "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Exception.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure() {
        // Act
        Result result = new[] { new ValidationError(nameof(result), "Some error.") };

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Exception.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure() {
        // Act
        Result result = new List<ValidationError> { new(nameof(result), "Some error.") };

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Exception.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorSet_ReturnsFailure() {
        // Act
        Result result = new HashSet<ValidationError> { new(nameof(result), "Some error.") };

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Exception.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_FromException_ReturnsError() {
        // Act
        Result result = new InvalidOperationException("Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEmpty();
        result.Exception.Should().NotBeNull();
    }

    [Fact]
    public void AddOperator_WithError_ReturnsInvalid() {
        // Arrange
        var result = Result.Success();

        // Act
        result += new ValidationError("result", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
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
            _invalidWithOtherError,
            _failure,
        };

        // Act
        var result = new HashSet<Result> {
            Result.Success(),
            Result.Success(),
            _success,
            _success,
            _invalid,
            _invalid,
            _invalidWithSameError,
            _invalidWithOtherError,
            _failure,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void OfT_CopyConstructor_ClonesObject() {
        // Act
        var result = _successWithValue with {
            Errors = new HashSet<ValidationError> { new("Some error.") },
        };

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
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
        var result = _successWithValue;

        // Act
        result += Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void AddOperator_WithValueAndWithError_ReturnsInvalid() {
        // Arrange
        var result = _successWithValue;

        // Act
        result += new ValidationError("result", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void MapTo_WithoutError_ReturnsSuccess() {
        // Arrange
        var subject = _successWithValue;

        // Act
        var result = subject.MapTo(s => s is null ? default : int.Parse(s));

        // Assert
        result.Should().BeOfType<Result<int>>();
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Exception.Should().BeNull();
    }

    [Fact]
    public void MapTo_WithError_ReturnsInvalid() {
        // Arrange
        var subject = _invalidWithValue;

        // Act
        var result = subject.MapTo(s => s is null ? default : int.Parse(s));

        // Assert
        result.Should().BeOfType<Result<int>>();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Exception.Should().BeNull();
    }

    [Fact]
    public void MapTo_WithException_ReturnsError() {
        // Arrange
        var subject = _failureWithValue;

        // Act
        var result = subject.MapTo(s => s is null ? (int?)null : int.Parse(s));

        // Assert
        result.Should().BeOfType<Result<int?>>();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEmpty();
        result.Exception.Should().NotBeNull();
    }
}
