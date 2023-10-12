namespace System.Results;

public class CrudResultTests {
    private static readonly ValidationError _error = new("Some {1} for {0}.", "Source", "error");

    private static readonly CrudResult _success = CrudResult.Success();
    private static readonly CrudResult _notFound = CrudResult.NotFound();
    private static readonly CrudResult _conflict = CrudResult.Conflict();
    private static readonly CrudResult _invalid = CrudResult.Invalid("Some {1} for {0}.", "Source", "error");
    private static readonly CrudResult _invalidFromResult = CrudResult.Invalid(ValidationResult.Failure(_error));
    private static readonly CrudResult _invalidFromError = CrudResult.Invalid(_error);
    private static readonly CrudResult _invalidFromErrors = CrudResult.Invalid(new[] { _error });
    private static readonly CrudResult _invalidWithMessageOnly = CrudResult.Invalid("Some error for Source.");
    private static readonly CrudResult _invalidWithSourceOnly = CrudResult.Invalid("Some error for {0}.", "Source");
    private static readonly CrudResult _invalidWithOtherSource = CrudResult.Invalid("Some {1} for {0}.", "OtherSource", "error");
    private static readonly CrudResult _invalidWithOtherData = CrudResult.Invalid("Some {1} for {0}.", "Source", "other error");
    private static readonly CrudResult _invalidWithOtherMessage = CrudResult.Invalid("Other {1} for {0}.", "Source", "error");

    [Fact]
    public void CloneConstructor_ReturnsInstance() {
        // Act
        var result = _success with { Errors = new[] { _error with { Source = "SomeField" } } };

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure() {
        // Act
        CrudResult result = new ValidationError("Some error {0}.", "Source");

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure() {
        // Act
        CrudResult result = new[] { new ValidationError("Some error {0}.", "Source") };

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromListOfValidationError_ReturnsFailure() {
        // Act
        CrudResult result = new List<ValidationError> { new("Some error {0}.", "Source") };

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_ToValidationResult_ReturnsFailure() {
        // Act
        ValidationResult result = _invalid;

        // Assert
        result.IsValid.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<CrudResult, bool, bool, bool, bool> {
        public TestDataForProperties() {
            Add(_invalid, true, false, false, false);
            Add(_success, false, true, false, false);
            Add(_notFound, false, false, true, false);
            Add(_conflict, false, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForProperties))]
    public void Properties_ShouldReturnAsExpected(ICrudResult subject, bool isInvalid, bool isSuccess, bool wasNotFound, bool hasConflict) {
        // Assert
        subject.IsFailure.Should().Be(isInvalid);
        subject.IsSuccess.Should().Be(isSuccess);
        subject.IsNotFound.Should().Be(wasNotFound);
        subject.IsConflict.Should().Be(hasConflict);
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
            Add(_invalid, _invalidFromResult, true);
            Add(_invalid, _invalidFromError, true);
            Add(_invalid, _invalidFromErrors, true);
            Add(_invalid, _invalidWithOtherMessage, false);
            Add(_invalid, _invalidWithOtherSource, false);
            Add(_invalid, _invalidWithOtherData, false);
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
            _invalidWithMessageOnly,
            _invalidWithOtherMessage,
        };

        // Act
        var result = new HashSet<CrudResult> {
            CrudResult.Success(),
            CrudResult.Success(),
            _success,
            _success,
            _invalidFromError,
            _invalidFromError,
            _invalid,
            _invalidWithOtherMessage,
            _invalid,
            _success,
            _invalidWithSourceOnly,
            _invalid,
            _success,
            _invalidWithMessageOnly,
            _invalidWithOtherMessage,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void AddOperator_WithoutError_ReturnsInvalid() {
        // Arrange
        var result = CrudResult.Success();

        // Act
        result += ValidationResult.Success();

        // Assert
        result.IsValid.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void AddOperator_WithError_ReturnsInvalid() {
        // Arrange
        var result = CrudResult<string>.Success("SomeToken");

        // Act
        result += new ValidationError("Some error {0}.", "Source");
        result += new ValidationError[] { new("Some error 3."), new("Some error 4.") };

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
    }

    [Fact]
    public void AddOperator_WithOtherError_ReturnsBothErrors() {
        // Arrange
        var result = CrudResult.Invalid("Some error {0}.", "Source");

        // Act
        result += new ValidationError("Other error. {0}", "Source");
        result += new ValidationError[] { new("Some error 3."), new("Some error 4.") };

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(4);
    }

    [Fact]
    public void AddOperator_WithSameError_ReturnsOnlyOneError() {
        // Arrange
        var result = CrudResult.Invalid("Some error {0}.", "Source");

        // Act
        result += new ValidationError("Some error {0}.", "Source");

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    private static readonly CrudResult<string> _successOfValue = CrudResult<string>.Success("Value");
    private static readonly CrudResult<string> _successOfValueWithOtherValue = CrudResult<string>.Success("Other");
    private static readonly CrudResult<string> _notFoundOfValue = CrudResult<string>.NotFound();
    private static readonly CrudResult<string> _conflictOfValue = CrudResult<string>.Conflict("Value");
    private static readonly CrudResult<string> _conflictOfValueWithOtherValue = CrudResult<string>.Conflict("Other");
    private static readonly CrudResult<string> _invalidOfValue = CrudResult<string>.Invalid("Some {1} for {0}.", "Source", "error");
    private static readonly CrudResult<string> _invalidOfValueFromResult = CrudResult<string>.Invalid(ValidationResult.Failure(_error));
    private static readonly CrudResult<string> _invalidOfValueFromError = CrudResult<string>.Invalid(_error);
    private static readonly CrudResult<string> _invalidOfValueFromErrors = CrudResult<string>.Invalid(new[] { _error });
    private static readonly CrudResult<string> _invalidOfValueWithMessageOnly = CrudResult<string>.Invalid("Some error for Source.");
    private static readonly CrudResult<string> _invalidOfValueWithSourceOnly = CrudResult<string>.Invalid("Some error for {0}.", "Source");
    private static readonly CrudResult<string> _invalidOfValueWithOtherError = CrudResult<string>.Invalid("Other {1} for {0}.", "Source", "error");
    private static readonly CrudResult<string> _invalidOfValueWithOtherSource = CrudResult<string>.Invalid("Some {1} for {0}.", "OtherSource", "error");
    private static readonly CrudResult<string> _invalidOfValueWithOtherData = CrudResult<string>.Invalid("Some {1} for {0}.", "Source", "other error");

    [Fact]
    public void CloneConstructor_WithValue_ReturnsInstance() {
        // Act
        var result = _successOfValue with { Errors = new[] { _error } };

        // Assert
        result.IsValid.Should().BeFalse();
    }

    private class TestDataForPropertiesWithValue : TheoryData<CrudResult<string>, bool, bool, bool, bool> {
        public TestDataForPropertiesWithValue() {
            Add(_invalidOfValue, true, false, false, false);
            Add(_successOfValue, false, true, false, false);
            Add(_notFoundOfValue, false, false, true, false);
            Add(_conflictOfValue, false, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForPropertiesWithValue))]
    public void Properties_WithValue_ShouldReturnAsExpected(CrudResult<string> subject, bool isInvalid, bool isSuccess, bool wasNotFound, bool hasConflict) {
        // Assert
        subject.IsFailure.Should().Be(isInvalid);
        subject.IsSuccess.Should().Be(isSuccess);
        subject.IsNotFound.Should().Be(wasNotFound);
        subject.IsConflict.Should().Be(hasConflict);
    }

    private class TestDataForEqualityWithValue : TheoryData<CrudResult<string>, CrudResult<string>?, bool> {
        public TestDataForEqualityWithValue() {
            Add(_successOfValue, null, false);
            Add(_successOfValue, _successOfValue, true);
            Add(_successOfValue, _successOfValueWithOtherValue, false);
            Add(_successOfValue, _notFoundOfValue, false);
            Add(_successOfValue, _conflictOfValue, false);
            Add(_successOfValue, _invalidOfValue, false);
            Add(_notFoundOfValue, null, false);
            Add(_notFoundOfValue, _successOfValue, false);
            Add(_notFoundOfValue, _notFoundOfValue, true);
            Add(_notFoundOfValue, _conflictOfValue, false);
            Add(_notFoundOfValue, _invalidOfValue, false);
            Add(_conflictOfValue, null, false);
            Add(_conflictOfValue, _successOfValue, false);
            Add(_conflictOfValue, _notFoundOfValue, false);
            Add(_conflictOfValue, _conflictOfValue, true);
            Add(_conflictOfValue, _conflictOfValueWithOtherValue, false);
            Add(_conflictOfValue, _invalidOfValue, false);
            Add(_invalidOfValue, null, false);
            Add(_invalidOfValue, _successOfValue, false);
            Add(_invalidOfValue, _notFoundOfValue, false);
            Add(_invalidOfValue, _conflictOfValue, false);
            Add(_invalidOfValue, _invalidOfValue, true);
            Add(_invalidOfValue, _invalidOfValueFromResult, true);
            Add(_invalidOfValue, _invalidOfValueFromError, true);
            Add(_invalidOfValue, _invalidOfValueFromErrors, true);
            Add(_invalidOfValue, _invalidOfValueWithSourceOnly, true);
            Add(_invalidOfValue, _invalidOfValueWithMessageOnly, false);
            Add(_invalidOfValue, _invalidOfValueWithSourceOnly, true);
            Add(_invalidOfValue, _invalidOfValueWithOtherError, false);
            Add(_invalidOfValue, _invalidOfValueWithOtherSource, false);
            Add(_invalidOfValue, _invalidOfValueWithOtherData, false);
        }
    }

    [Theory]
    [ClassData(typeof(TestDataForEqualityWithValue))]
    public void Equals_WithValue_ReturnsAsExpected(CrudResult<string> subject, CrudResult<string>? other, bool expectedResult) {
        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEqualityWithValue))]
    public void NotEquals_WithValue_ReturnsAsExpected(CrudResult<string> subject, CrudResult<string>? other, bool expectedResult) {
        // Act
        var result = subject != other;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_WithValue_DifferentiatesAsExpected() {
        var expectedResult = new HashSet<CrudResult<string>> {
            _successOfValue,
            _successOfValueWithOtherValue,
            _invalidOfValue,
            _invalidOfValueWithOtherError,
        };

        // Act
        var result = new HashSet<CrudResult<string>> {
            CrudResult<string>.Success("Value"),
            CrudResult<string>.Success("Other"),
            _successOfValue,
            _successOfValue,
            _invalidOfValue,
            _invalidOfValue,
            _invalidOfValueWithSourceOnly,
            _invalidOfValueWithOtherError,
            _invalidOfValueWithSourceOnly,
            _successOfValueWithOtherValue,
            _invalidOfValueWithOtherError,
            _successOfValue,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ReturnsSuccess() {
        // Act
        CrudResult<string> subject = "Value";

        // Assert
        subject.Value.Should().Be("Value");
        subject.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_ToValidationResult_WithValue_ReturnsFailure() {
        // Act
        ValidationResult result = _invalidOfValue;

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromListOfValidationError_WithValue_ReturnsFailure() {
        // Act
        CrudResult<string> result = new List<ValidationError> { new("Some error {0}.", "Source") };

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void AddOperator_OfValueAndWithoutError_ReturnsInvalid() {
        // Arrange
        var result = CrudResult<string>.Success("Value");

        // Act
        result += ValidationResult.Success();

        // Assert
        result.IsValid.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void AddOperator_OfValueAndWithError_ReturnsInvalid() {
        // Arrange
        var result = CrudResult<string>.Success("Value");

        // Act
        result += new ValidationError("Some error {0}.", "Source");

        // Assert
        result.IsValid.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void MapTo_WithoutError_ReturnsSuccess() {
        // Arrange
        var subject = CrudResult<string>.Success("42");

        // Act
        var result = subject.Map(int.Parse);

        // Assert
        result.Should().BeOfType<CrudResult<int>>();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void MapTo_WithNotFound_ReturnsNotFound() {
        // Arrange
        var subject = CrudResult<string>.NotFound();

        // Act
        var result = subject.Map(int.Parse);

        // Assert
        result.Should().BeOfType<CrudResult<int>>();
        result.IsNotFound.Should().BeTrue();
        result.Value.Should().Be(0);
    }
}
