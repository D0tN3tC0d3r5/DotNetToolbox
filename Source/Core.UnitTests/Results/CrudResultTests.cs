using static DotNetToolbox.Results.CrudResult;

namespace DotNetToolbox.Results;

public class CrudResultTests {
    private static readonly CrudResult _success = Success();
    private static readonly CrudResult _notFound = NotFound();
    private static readonly CrudResult _conflict = Conflict();
    private static readonly CrudResult _invalid = Invalid(new ValidationError("Some error.", "Source"));
    private static readonly CrudResult _invalidWithSameError = new ValidationError("Some error.", "Source");
    private static readonly CrudResult _invalidWithWithOtherError = new ValidationError("Other error.", "Source");
    private static readonly CrudResult _failure = Error(new("Some error."));

    private static readonly CrudResult<string> _successWithValue = Success("Value");
    private static readonly CrudResult<string> _notFoundWithValue = NotFound<string>();
    private static readonly CrudResult<string> _conflictWithValue = Conflict("Value");
    private static readonly CrudResult<string> _invalidWithValue = Invalid("Value", new ValidationError("Some error.", "Source"));
    private static readonly CrudResult<string> _failureWithValue = Error<string>(new("Some error."));

    [Fact]
    public void CopyConstructor_ClonesObject() {
        // Act
        var result = _success with {
            Errors = new List<ValidationError> { new("Some error.") },
        };

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure() {
        // Act
        CrudResult result = new ValidationError("Some error.", nameof(result));

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure() {
        // Act
        CrudResult result = new[] { new ValidationError("Some error.", nameof(result)) };

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure() {
        // Act
        CrudResult result = new List<ValidationError> { new("Some error.", nameof(result)) };

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<CrudResult, bool, bool, bool> {
        public TestDataForProperties() {
            Add(_invalid, false, false, false);
            Add(_failure, false, false, false);
            Add(_success, true, false, false);
            Add(_notFound, false, true, false);
            Add(_conflict, false, false, true);
            Add(_invalidWithValue, false, false, false);
            Add(_failureWithValue, false, false, false);
            Add(_successWithValue, true, false, false);
            Add(_notFoundWithValue, false, true, false);
            Add(_conflictWithValue, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForProperties))]
    public void Properties_ShouldReturnAsExpected(CrudResult subject, bool isSuccess, bool isNotFound, bool isConflict) {
        // Assert
        subject.IsSuccess.Should().Be(isSuccess);
        subject.WasNotFound.Should().Be(isNotFound);
        subject.HasConflict.Should().Be(isConflict);
    }

    private class TestDataForEquality : TheoryData<CrudResult, CrudResult?, bool> {
        public TestDataForEquality() {
            Add(_success, null, false);
            Add(_success, _success, true);
            Add(_success, _notFound, false);
            Add(_success, _conflict, false);
            Add(_success, _invalid, false);
            Add(_notFound, null, false);
            Add(_notFound, _success, false);
            Add(_notFound, _notFound, true);
            Add(_notFound, _conflict, false);
            Add(_notFound, _invalid, false);
            Add(_conflict, null, false);
            Add(_conflict, _success, false);
            Add(_conflict, _notFound, false);
            Add(_conflict, _conflict, true);
            Add(_conflict, _invalid, false);
            Add(_invalid, null, false);
            Add(_invalid, _success, false);
            Add(_invalid, _notFound, false);
            Add(_invalid, _conflict, false);
            Add(_invalid, _invalid, true);
            Add(_invalid, _invalidWithSameError, true);
            Add(_invalid, _invalidWithWithOtherError, false);
        }
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void Equals_ReturnsAsExpected(CrudResult subject, CrudResult? other, bool expectedResult) {
        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void NotEquals_ReturnsAsExpected(CrudResult subject, CrudResult? other, bool expectedResult) {
        // Act
        var result = subject != other;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_DifferentiatesAsExpected() {
        var expectedResult = new HashSet<CrudResult> {
            _success,
            _invalid,
            _invalidWithWithOtherError,
        };

        // Act
        var result = new HashSet<CrudResult> {
            Success(),
            Success(),
            _success,
            _success,
            _invalid,
            _invalid,
            _invalidWithSameError,
            _invalidWithWithOtherError,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Success_CreatesResult() {
        // Arrange & Act
        var result = Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Invalid_WithMessageOnly_CreatesResult() {
        // Arrange & Act
        var result = Invalid(new ValidationError("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Invalid_WithSourceAndMessage_CreatesResult() {
        // Arrange & Act
        var result = Invalid(new ValidationError("Some error.", "Field1"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new[] {
            new ValidationError("Some error.", "Field1"),
        });
    }

    [Fact]
    public void Invalid_WithResult_CreatesResult() {
        // Arrange & Act
        var result = Invalid(Result.Invalid("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void AddOperator_WithoutError_ReturnsInvalid() {
        // Arrange
        var result = Success();

        // Act
        result += Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void AddOperator_WithError_ReturnsInvalid() {
        // Arrange
        var result = Success("SomeToken");

        // Act
        result += new ValidationError("Some error.", "Source");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void AddOperator_WithOtherError_ReturnsBothErrors() {
        // Arrange
        var result = Invalid(new ValidationError("Some error 42.", "Source"));

        // Act
        result += new ValidationError("Other error 42.", "Source");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void AddOperator_WithSameError_ReturnsOnlyOneError() {
        // Arrange
        var result = Invalid(new ValidationError("Some error.", "Source"));

        // Act
        result += new ValidationError("Some error.", "Source");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void OfT_CopyConstructor_ClonesObject() {
        // Arrange
        var original = Success(42);

        // Act
        var result = original with {
            Value = 7,
        };

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(7);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ReturnsSuccess() {
        // Act
        CrudResult<string> subject = "Value";

        // Assert
        subject.Value.Should().Be("Value");
        subject.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromSuccessResult_ReturnsSuccess() {
        // Act
        var result = Result.Success("Value");
        CrudResult<string> subject = result;

        // Assert
        subject.Value.Should().Be(result.Value);
        subject.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromInvalidResult_ReturnsSuccess() {
        // Act
        var result = Result.Invalid("Value", "Some error.", "SomeProperty");
        CrudResult<string> subject = result;

        // Assert
        subject.Value.Should().Be(result.Value);
        subject.IsSuccess.Should().BeFalse();
        subject.Errors.Should().BeEquivalentTo(result.Errors);
    }

    [Fact]
    public void AddOperator_WithValueAndWithoutError_ReturnsInvalid() {
        // Arrange
        var result = Success("Value");

        // Act
        result += Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
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
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void MapTo_WithoutError_ReturnsSuccess() {
        // Arrange
        var subject = Success("42");

        // Act
        var result = subject.MapTo(s => s is null ? default : int.Parse(s));

        // Assert
        result.Should().BeOfType<CrudResult<int>>();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void MapTo_FromNotFound_ReturnsSuccess() {
        // Arrange
        var subject = NotFound<string>();

        // Act
        var result = subject.MapTo(s => s is null ? default : int.Parse(s));

        // Assert
        result.Should().BeOfType<CrudResult<int>>();
        result.IsSuccess.Should().BeFalse();
        result.WasNotFound.Should().BeTrue();
    }

    [Fact]
    public void MapTo_WithError_ReturnsInvalid() {
        // Arrange
        var subject = Invalid("42", new ValidationError("Some error.", "Field1"));

        // Act
        var result = subject.MapTo(s => s is null ? default : int.Parse(s));

        // Assert
        result.Should().BeOfType<CrudResult<int>>();
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void SuccessOfT_CreatesResult() {
        // Arrange & Act
        var result = Success(42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void InvalidOfT_WithMessageOnly_CreatesResult() {
        // Arrange & Act
        var result = Invalid(42, new ValidationError("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void InvalidOfT_WithSourceAndMessage_CreatesResult() {
        // Arrange & Act
        var result = Invalid(42, new ValidationError("Some error.", "Field1"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new[] {
            new ValidationError("Some error.", "Field1"),
        });
    }

    [Fact]
    public void InvalidOfT_WithResult_CreatesResult() {
        // Arrange & Act
        var result = Invalid(42, Result.Invalid("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }
}
