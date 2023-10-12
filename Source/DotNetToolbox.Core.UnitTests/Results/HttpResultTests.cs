namespace System.Results;

public class HttpResultTests {
    private static readonly ValidationError _error = new("Some {1} for {0}.", "Source", "error");

    private static readonly HttpResult _ok = HttpResult.Ok();
    private static readonly HttpResult _created = HttpResult.Created();
    private static readonly HttpResult _unauthorized = HttpResult.Unauthorized();
    private static readonly HttpResult _notFound = HttpResult.NotFound();
    private static readonly HttpResult _conflict = HttpResult.Conflict();
    private static readonly HttpResult _badRequest = HttpResult.BadRequest("Some {1} for {0}.", "Source", "error");
    private static readonly HttpResult _badRequestFromResult = HttpResult.BadRequest(ValidationResult.Failure(_error));
    private static readonly HttpResult _badRequestFromError = HttpResult.BadRequest(_error);
    private static readonly HttpResult _badRequestFromErrors = HttpResult.BadRequest(new[] { _error });
    private static readonly HttpResult _badRequestWithMessageOnly = HttpResult.BadRequest("Some error for Source.");
    private static readonly HttpResult _badRequestWithSourceOnly = HttpResult.BadRequest("Some error for {0}.", "Source");
    private static readonly HttpResult _badRequestWithOtherSource = HttpResult.BadRequest("Some {1} for {0}.", "OtherSource", "error");
    private static readonly HttpResult _badRequestWithOtherData = HttpResult.BadRequest("Some {1} for {0}.", "Source", "other error");
    private static readonly HttpResult _badRequestWithOtherMessage = HttpResult.BadRequest("Other {1} for {0}.", "Source", "error");

    [Fact]
    public void CloneConstructor_ReturnsInstance() {
        // Act
        var result = _ok with { Errors = new[] { _error } };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure() {
        // Act
        HttpResult result = _error;

        // Assert
        result.IsOk.Should().BeFalse();
        result.IsBadRequest.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure() {
        // Act
        HttpResult result = new[] { _error };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure() {
        // Act
        HttpResult result = new List<ValidationError> { _error };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromErrorDictionary_ReturnsFailure() {
        // Act
        HttpResult result = new Dictionary<string, string[]> { ["Source"] = new[] { "Some error." } };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_ToValidationResult_ReturnsFailure() {
        // Act
        ValidationResult result = _badRequest;

        // Assert
        result.IsValid.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<IHttpResult, bool, bool, bool, bool, bool, bool> {
        public TestDataForProperties() {
            Add(_badRequest, true, false, false, false, false, false);
            Add(_ok, false, true, false, false, false, false);
            Add(_notFound, false, false, true, false, false, false);
            Add(_conflict, false, false, false, true, false, false);
            Add(_created, false, false, false, false, true, false);
            Add(_unauthorized, false, false, false, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForProperties))]
    public void Properties_ShouldReturnAsExpected(IHttpResult subject, bool isBadRequest, bool isOk, bool isNotFound, bool isConflict, bool isCreated, bool isUnauthorized) {
        // Assert
        subject.IsBadRequest.Should().Be(isBadRequest);
        subject.IsOk.Should().Be(isOk);
        subject.IsNotFound.Should().Be(isNotFound);
        subject.IsConflict.Should().Be(isConflict);
        subject.IsCreated.Should().Be(isCreated);
        subject.IsUnauthorized.Should().Be(isUnauthorized);
    }

    private class TestDataForEquality : TheoryData<HttpResult, HttpResult?, bool> {
        public TestDataForEquality() {
            Add(_ok, null, false);
            Add(_ok, _ok, true);
            Add(_ok, _created, false);
            Add(_ok, _notFound, false);
            Add(_ok, _unauthorized, false);
            Add(_ok, _conflict, false);
            Add(_ok, _badRequest, false);
            Add(_created, null, false);
            Add(_created, _ok, false);
            Add(_created, _created, true);
            Add(_created, _notFound, false);
            Add(_created, _unauthorized, false);
            Add(_created, _conflict, false);
            Add(_created, _badRequest, false);
            Add(_notFound, null, false);
            Add(_notFound, _ok, false);
            Add(_notFound, _created, false);
            Add(_notFound, _notFound, true);
            Add(_notFound, _unauthorized, false);
            Add(_notFound, _conflict, false);
            Add(_notFound, _badRequest, false);
            Add(_unauthorized, null, false);
            Add(_unauthorized, _ok, false);
            Add(_unauthorized, _created, false);
            Add(_unauthorized, _notFound, false);
            Add(_unauthorized, _unauthorized, true);
            Add(_unauthorized, _conflict, false);
            Add(_unauthorized, _badRequest, false);
            Add(_conflict, null, false);
            Add(_conflict, _ok, false);
            Add(_conflict, _created, false);
            Add(_conflict, _notFound, false);
            Add(_conflict, _unauthorized, false);
            Add(_conflict, _conflict, true);
            Add(_conflict, _badRequest, false);
            Add(_badRequest, null, false);
            Add(_badRequest, _ok, false);
            Add(_badRequest, _created, false);
            Add(_badRequest, _notFound, false);
            Add(_badRequest, _unauthorized, false);
            Add(_badRequest, _conflict, false);
            Add(_badRequest, _badRequest, true);
            Add(_badRequest, _badRequestFromResult, true);
            Add(_badRequest, _badRequestFromError, true);
            Add(_badRequest, _badRequestFromErrors, true);
            Add(_badRequest, _badRequestWithOtherMessage, false);
            Add(_badRequest, _badRequestWithOtherSource, false);
            Add(_badRequest, _badRequestWithOtherData, false);
        }
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void Equals_ReturnsAsExpected(HttpResult subject, HttpResult? other, bool expectedResult) {
        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void NotEquals_ReturnsAsExpected(HttpResult subject, HttpResult? other, bool expectedResult) {
        // Act
        var result = subject != other;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_DifferentiatesAsExpected() {
        var expectedResult = new HashSet<HttpResult> {
            _ok,
            _badRequest,
            _badRequestWithMessageOnly,
            _badRequestWithOtherMessage,
        };

        // Act
        var result = new HashSet<HttpResult> {
            HttpResult.Ok(),
            HttpResult.Ok(),
            _ok,
            _ok,
            _badRequestFromError,
            _badRequestFromError,
            _badRequest,
            _badRequestWithOtherMessage,
            _badRequest,
            _ok,
            _badRequestWithSourceOnly,
            _badRequest,
            _ok,
            _badRequestWithMessageOnly,
            _badRequestWithOtherMessage,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void AddOperator_WithoutError_ReturnsBadRequest() {
        // Arrange
        var result = HttpResult.Ok();

        // Act
        result += ValidationResult.Success();

        // Assert
        result.IsOk.Should().BeTrue();
        result.IsBadRequest.Should().BeFalse();
    }

    [Fact]
    public void AddOperator_WithError_ReturnsBadRequest() {
        // Arrange
        var result = HttpResult<string>.Ok("SomeToken");

        // Act
        result += _error;

        // Assert
        result.IsOk.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void AddOperator_WithOtherError_ReturnsBothErrors() {
        // Arrange
        var result = _badRequest;

        // Act
        result += new ValidationError("Other error {0}.", "Source");
        result += new ValidationError[] { new("Some error 3."), new("Some error 4.") };

        // Assert
        result.IsOk.Should().BeFalse();
        result.Errors.Should().HaveCount(4);
    }

    [Fact]
    public void AddOperator_WithSameError_ReturnsOnlyOneError() {
        // Arrange
        var result = _badRequest;

        // Act
        result += _error;

        // Assert
        result.IsOk.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    private static readonly HttpResult<string> _okOfValue = HttpResult<string>.Ok("Value");
    private static readonly HttpResult<string> _okOfValueWithOtherValue = HttpResult<string>.Ok("Other");
    private static readonly HttpResult<string> _createdOfValue = HttpResult<string>.Created("Value");
    private static readonly HttpResult<string> _createdOfValueWithOtherValue = HttpResult<string>.Created("Other");
    private static readonly HttpResult<string> _unauthorizedOfValue = HttpResult<string>.Unauthorized();
    private static readonly HttpResult<string> _notFoundOfValue = HttpResult<string>.NotFound();
    private static readonly HttpResult<string> _conflictOfValue = HttpResult<string>.Conflict("Value");
    private static readonly HttpResult<string> _conflictOfValueWithDefault = HttpResult<string>.Conflict(default);
    private static readonly HttpResult<string> _badRequestOfValue = HttpResult<string>.BadRequest("Some {1} for {0}.", "Source", "error");
    private static readonly HttpResult<string> _badRequestOfValueFromResult = HttpResult<string>.BadRequest(ValidationResult.Failure(_error));
    private static readonly HttpResult<string> _badRequestOfValueFromError = HttpResult<string>.BadRequest(_error);
    private static readonly HttpResult<string> _badRequestOfValueFromErrors = HttpResult<string>.BadRequest(new[] { _error });
    private static readonly HttpResult<string> _badRequestOfValueWithMessageOnly = HttpResult<string>.BadRequest("Some error for Source.");
    private static readonly HttpResult<string> _badRequestOfValueWithSourceOnly = HttpResult<string>.BadRequest("Some error for {0}.", "Source");
    private static readonly HttpResult<string> _badRequestOfValueWithOtherError = HttpResult<string>.BadRequest("Other {1} for {0}.", "Source", "error");
    private static readonly HttpResult<string> _badRequestOfValueWithOtherSource = HttpResult<string>.BadRequest("Some {1} for {0}.", "OtherSource", "error");
    private static readonly HttpResult<string> _badRequestOfValueWithOtherData = HttpResult<string>.BadRequest("Some {1} for {0}.", "Source", "other error");

    [Fact]
    public void CloneConstructor_OfValue_ReturnsInstance() {
        // Act
        var result = _okOfValue with { Errors = new[] { _error } };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    private class TestDataForPropertiesOfValue : TheoryData<HttpResult<string>, bool, bool, bool, bool, bool, bool> {
        public TestDataForPropertiesOfValue() {
            Add(_badRequestOfValue, true, false, false, false, false, false);
            Add(_okOfValue, false, true, false, false, false, false);
            Add(_notFoundOfValue, false, false, true, false, false, false);
            Add(_conflictOfValue, false, false, false, true, false, false);
            Add(_createdOfValue, false, false, false, false, true, false);
            Add(_unauthorizedOfValue, false, false, false, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForPropertiesOfValue))]
    public void Properties_OfValue_ShouldReturnAsExpected(HttpResult<string> subject, bool isBadRequest, bool isOk, bool isNotFound, bool isConflict, bool isCreated, bool isUnauthorized) {
        // Assert
        subject.IsBadRequest.Should().Be(isBadRequest);
        subject.IsOk.Should().Be(isOk);
        subject.IsNotFound.Should().Be(isNotFound);
        subject.IsConflict.Should().Be(isConflict);
        subject.IsCreated.Should().Be(isCreated);
        subject.IsUnauthorized.Should().Be(isUnauthorized);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ReturnsOk() {
        // Act
        HttpResult<string> subject = "Value";

        // Assert
        subject.Value.Should().Be("Value");
        subject.IsOk.Should().BeTrue();
    }

    [Fact]
    public void AddOperator_OfValueAndWithoutError_ReturnsBadRequest() {
        // Arrange
        var result = HttpResult<string>.Ok("Value");

        // Act
        result += ValidationResult.Success();

        // Assert
        result.IsOk.Should().BeTrue();
        result.IsBadRequest.Should().BeFalse();
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void AddOperator_OfValueAndWithError_ReturnsBadRequest() {
        // Arrange
        var result = HttpResult<string>.Ok("Value");

        // Act
        result += _error;
        result += new ValidationError[] { new("Some error 3."), new("Some error 4.") };

        // Assert
        result.IsOk.Should().BeFalse();
        result.IsBadRequest.Should().BeTrue();
        result.Value.Should().Be("Value");
        result.Errors.Should().HaveCount(3);
    }

    [Fact]
    public void MapTo_WithoutError_ReturnsOk() {
        // Arrange
        var subject = HttpResult<string>.Ok("42");

        // Act
        var result = subject.Map(int.Parse);

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeTrue();
    }

    [Fact]
    public void MapTo_FromNotFound_ReturnsOk() {
        // Arrange
        var subject = HttpResult<string>.NotFound();

        // Act
        var result = subject.Map(int.Parse);

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeFalse();
        result.IsBadRequest.Should().BeFalse();
        result.IsNotFound.Should().BeTrue();
    }

    [Fact]
    public void MapTo_WithError_ReturnsBadRequest() {
        // Arrange
        var subject = HttpResult<string>.BadRequest("Some error {0}.", "Source");

        // Act
        var result = subject.Map(int.Parse);

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeFalse();
    }

    private class TestDataForEqualityOfValue : TheoryData<HttpResult<string>, HttpResult<string>?, bool> {
        public TestDataForEqualityOfValue() {
            Add(_okOfValue, null, false);
            Add(_okOfValue, _okOfValue, true);
            Add(_okOfValue, _okOfValueWithOtherValue, false);
            Add(_okOfValue, _createdOfValue, false);
            Add(_okOfValue, _notFoundOfValue, false);
            Add(_okOfValue, _unauthorizedOfValue, false);
            Add(_okOfValue, _conflictOfValue, false);
            Add(_okOfValue, _badRequestOfValue, false);
            Add(_createdOfValue, null, false);
            Add(_createdOfValue, _okOfValue, false);
            Add(_createdOfValue, _createdOfValue, true);
            Add(_createdOfValue, _createdOfValueWithOtherValue, false);
            Add(_createdOfValue, _notFoundOfValue, false);
            Add(_createdOfValue, _unauthorizedOfValue, false);
            Add(_createdOfValue, _conflictOfValue, false);
            Add(_createdOfValue, _badRequestOfValue, false);
            Add(_notFoundOfValue, null, false);
            Add(_notFoundOfValue, _okOfValue, false);
            Add(_notFoundOfValue, _createdOfValue, false);
            Add(_notFoundOfValue, _notFoundOfValue, true);
            Add(_notFoundOfValue, _unauthorizedOfValue, false);
            Add(_notFoundOfValue, _conflictOfValue, false);
            Add(_notFoundOfValue, _badRequestOfValue, false);
            Add(_unauthorizedOfValue, null, false);
            Add(_unauthorizedOfValue, _okOfValue, false);
            Add(_unauthorizedOfValue, _createdOfValue, false);
            Add(_unauthorizedOfValue, _notFoundOfValue, false);
            Add(_unauthorizedOfValue, _unauthorizedOfValue, true);
            Add(_unauthorizedOfValue, _conflictOfValue, false);
            Add(_unauthorizedOfValue, _badRequestOfValue, false);
            Add(_conflictOfValue, null, false);
            Add(_conflictOfValue, _okOfValue, false);
            Add(_conflictOfValue, _createdOfValue, false);
            Add(_conflictOfValue, _notFoundOfValue, false);
            Add(_conflictOfValue, _unauthorizedOfValue, false);
            Add(_conflictOfValue, _conflictOfValue, true);
            Add(_conflictOfValue, _conflictOfValueWithDefault, false);
            Add(_conflictOfValue, _badRequestOfValue, false);
            Add(_badRequestOfValue, null, false);
            Add(_badRequestOfValue, _okOfValue, false);
            Add(_badRequestOfValue, _createdOfValue, false);
            Add(_badRequestOfValue, _notFoundOfValue, false);
            Add(_badRequestOfValue, _unauthorizedOfValue, false);
            Add(_badRequestOfValue, _conflictOfValue, false);
            Add(_badRequestOfValue, _badRequestOfValue, true);
            Add(_badRequestOfValue, _badRequestOfValueFromResult, true);
            Add(_badRequestOfValue, _badRequestOfValueFromError, true);
            Add(_badRequestOfValue, _badRequestOfValueFromErrors, true);
            Add(_badRequestOfValue, _badRequestOfValueWithSourceOnly, true);
            Add(_badRequestOfValue, _badRequestOfValueWithMessageOnly, false);
            Add(_badRequestOfValue, _badRequestOfValueWithSourceOnly, true);
            Add(_badRequestOfValue, _badRequestOfValueWithOtherError, false);
            Add(_badRequestOfValue, _badRequestOfValueWithOtherSource, false);
            Add(_badRequestOfValue, _badRequestOfValueWithOtherData, false);
        }
    }

    [Fact]
    public void ImplicitConversion_ToValidationResult_OfValue_ReturnsFailure() {
        // Act
        ValidationResult result = _badRequestOfValue;

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromListOfValidationError_OfValue_ReturnsFailure() {
        // Act
        HttpResult<string> result = new List<ValidationError> { new("Some error {0}.", "Source") };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(TestDataForEqualityOfValue))]
    public void Equals_OfValue_ReturnsAsExpected(HttpResult<string> subject, HttpResult<string>? other, bool expectedResult) {
        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEqualityOfValue))]
    public void NotEquals_OfValue_ReturnsAsExpected(HttpResult<string> subject, HttpResult<string>? other, bool expectedResult) {
        // Act
        var result = subject != other;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_OfValue_DifferentiatesAsExpected() {
        var expectedResult = new HashSet<HttpResult<string>> {
            _okOfValue,
            _okOfValueWithOtherValue,
            _badRequestOfValue,
            _badRequestOfValueWithOtherError,
        };

        // Act
        var result = new HashSet<HttpResult<string>> {
            HttpResult<string>.Ok("Value"),
            HttpResult<string>.Ok("Other"),
            _okOfValue,
            _okOfValue,
            _badRequestOfValue,
            _badRequestOfValue,
            _badRequestOfValueWithSourceOnly,
            _badRequestOfValueWithOtherError,
            _badRequestOfValueWithSourceOnly,
            _okOfValueWithOtherValue,
            _badRequestOfValueWithOtherError,
            _okOfValue,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
