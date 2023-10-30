namespace System.Results;

public class CrudResultTests {
    private static readonly CrudResult _success = CrudResult.Success();
    private static readonly CrudResult _notFound = CrudResult.NotFound();
    private static readonly CrudResult _conflict = CrudResult.Conflict();
    private static readonly CrudResult _invalid = CrudResult.Invalid("Some error.", "Source");
    private static readonly CrudResult _invalidWithSameError = new ValidationError("Source", "Some error.");
    private static readonly CrudResult _invalidWithWithOtherError = new ValidationError("Source", "Other error.");

    private static readonly CrudResult<string> _successWithValue = CrudResult.Success("Value");
    private static readonly CrudResult<string> _notFoundWithValue = CrudResult.NotFound<string>();
    private static readonly CrudResult<string> _conflictWithValue = CrudResult.Conflict("Value");
    private static readonly CrudResult<string> _invalidWithValue = CrudResult.Invalid("Value", "Some error.", "Source");

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure() {
        // Act
        CrudResult result = new ValidationError(nameof(result), "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure() {
        // Act
        CrudResult result = new[] { new ValidationError(nameof(result), "Some error.") };

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure() {
        // Act
        CrudResult result = new List<ValidationError> { new(nameof(result), "Some error.") };

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<CrudResult, bool, bool, bool, bool> {
        public TestDataForProperties() {
            Add(_invalid, true, false, false, false);
            Add(_success, false, true, false, false);
            Add(_notFound, false, false, true, false);
            Add(_conflict, false, false, false, true);
            Add(_invalidWithValue, true, false, false, false);
            Add(_successWithValue, false, true, false, false);
            Add(_notFoundWithValue, false, false, true, false);
            Add(_conflictWithValue, false, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForProperties))]
    public void Properties_ShouldReturnAsExpected(CrudResult subject, bool isInvalid, bool isSuccess, bool isNotFound, bool isConflict) {
        // Assert
        subject.IsInvalid.Should().Be(isInvalid);
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
            _invalidWithWithOtherError
        };

        // Act
        var result = new HashSet<CrudResult> {
            CrudResult.Success(),
            CrudResult.Success(),
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
    public void AddOperator_WithoutError_ReturnsInvalid() {
        // Arrange
        var result = CrudResult.Success();

        // Act
        result += Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsInvalid.Should().BeFalse();
    }

    [Fact]
    public void AddOperator_WithError_ReturnsInvalid() {
        // Arrange
        var result = CrudResult.Success("SomeToken");

        // Act
        result += new ValidationError("Source", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void AddOperator_WithOtherError_ReturnsBothErrors() {
        // Arrange
        var result = CrudResult.Invalid("Some error.", "Source");

        // Act
        result += new ValidationError("Source", "Other error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void AddOperator_WithSameError_ReturnsOnlyOneError() {
        // Arrange
        var result = CrudResult.Invalid("Some error.", "Source");

        // Act
        result += new ValidationError("Source", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
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
        var result = CrudResult.Success("Value");

        // Act
        result += Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsInvalid.Should().BeFalse();
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void AddOperator_WithValueAndWithError_ReturnsInvalid() {
        // Arrange
        var result = CrudResult.Success("Value");

        // Act
        result += new ValidationError("result", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsInvalid.Should().BeTrue();
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void MapTo_WithoutError_ReturnsSuccess() {
        // Arrange
        var subject = CrudResult.Success("42");

        // Act
        var result = subject.MapTo(int.Parse);

        // Assert
        result.Should().BeOfType<CrudResult<int>>();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void MapTo_FromNotFound_ReturnsSuccess() {
        // Arrange
        var subject = CrudResult.NotFound<string>();

        // Act
        var result = subject.MapTo(int.Parse);

        // Assert
        result.Should().BeOfType<CrudResult<int>>();
        result.IsSuccess.Should().BeFalse();
        result.IsInvalid.Should().BeFalse();
        result.WasNotFound.Should().BeTrue();
    }

    [Fact]
    public void MapTo_WithError_ReturnsInvalid() {
        // Arrange
        var subject = CrudResult.Invalid("42", "Some error.", "Field");

        // Act
        var result = subject.MapTo(int.Parse);

        // Assert
        result.Should().BeOfType<CrudResult<int>>();
        result.IsSuccess.Should().BeFalse();
    }
}
