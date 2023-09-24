namespace DotNetToolbox.Results;

public class HttpResultTests
{
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
    public void ImplicitConversion_FromValidationError_ReturnsFailure()
    {
        // Act
        HttpResult result = _error;

        // Assert
        result.IsOk.Should().BeFalse();
        result.IsBadRequest.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure()
    {
        // Act
        HttpResult result = new[] { _error };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure()
    {
        // Act
        HttpResult result = new List<IValidationError> { _error };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromErrorDictionary_ReturnsFailure()
    {
        // Act
        HttpResult result = new Dictionary<string, string[]> { ["Source"] = new[] { "Some error." }  };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_ToValidationResult_ReturnsFailure()
    {
        // Act
        ValidationResult result = _badRequest;

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<IHttpResult, bool, bool, bool, bool, bool, bool>
    {
        public TestDataForProperties()
        {
            Add(_badRequest,           true, false, false, false, false, false);
            Add(_ok,                   false, true, false, false, false, false);
            Add(_notFound,             false, false, true, false, false, false);
            Add(_conflict,             false, false, false, true, false, false);
            Add(_created,              false, false, false, false, true, false);
            Add(_unauthorized,         false, false, false, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForProperties))]
    public void Properties_ShouldReturnAsExpected(IHttpResult subject, bool isBadRequest, bool isOk, bool isNotFound, bool isConflict, bool isCreated, bool isUnauthorized)
    {
        // Assert
        subject.IsBadRequest.Should().Be(isBadRequest);
        subject.IsOk.Should().Be(isOk);
        subject.IsNotFound.Should().Be(isNotFound);
        subject.IsConflict.Should().Be(isConflict);
        subject.IsCreated.Should().Be(isCreated);
        subject.IsUnauthorized.Should().Be(isUnauthorized);
    }

    private class TestDataForEquality : TheoryData<HttpResult, HttpResult?, bool>
    {
        public TestDataForEquality()
        {
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
    public void Equals_ReturnsAsExpected(HttpResult subject, HttpResult? other, bool expectedResult)
    {
        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void NotEquals_ReturnsAsExpected(HttpResult subject, HttpResult? other, bool expectedResult)
    {
        // Act
        var result = subject != other;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_DifferentiatesAsExpected()
    {
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
    public void AddOperator_WithoutError_ReturnsBadRequest()
    {
        // Arrange
        var result = HttpResult.Ok();

        // Act
        result += ValidationResult.Success();

        // Assert
        result.IsOk.Should().BeTrue();
        result.IsBadRequest.Should().BeFalse();
    }

    [Fact]
    public void AddOperator_WithError_ReturnsBadRequest()
    {
        // Arrange
        var result = HttpResult<string>.Ok("SomeToken");

        // Act
        result += _error;

        // Assert
        result.IsOk.Should().BeFalse();
        result.ValidationErrors.Should().HaveCount(1);
    }

    [Fact]
    public void AddOperator_WithOtherError_ReturnsBothErrors()
    {
        // Arrange
        var result = _badRequest;

        // Act
        result += new ValidationError("Other error {0}.", "Source");
        result += new ValidationError[] { new("Some error 3."), new("Some error 4.") };

        // Assert
        result.IsOk.Should().BeFalse();
        result.ValidationErrors.Should().HaveCount(4);
    }

    [Fact]
    public void AddOperator_WithSameError_ReturnsOnlyOneError()
    {
        // Arrange
        var result = _badRequest;

        // Act
        result += _error;

        // Assert
        result.IsOk.Should().BeFalse();
        result.ValidationErrors.Should().HaveCount(1);
    }

    private static readonly HttpResult<string> _okWithValue = HttpResult<string>.Ok("Value");
    private static readonly HttpResult<string> _okWithOtherValue = HttpResult<string>.Ok("Other");
    private static readonly HttpResult<string> _createdWithValue = HttpResult<string>.Created("Value");
    private static readonly HttpResult<string> _createdWithOtherValue = HttpResult<string>.Created("Other");
    private static readonly HttpResult<string> _unauthorizedForValue = HttpResult<string>.Unauthorized();
    private static readonly HttpResult<string> _notFoundForValue = HttpResult<string>.NotFound();
    private static readonly HttpResult<string> _conflictForValue = HttpResult<string>.Conflict("Value");
    private static readonly HttpResult<string> _conflictForOtherValue = HttpResult<string>.Conflict(default);
    private static readonly HttpResult<string> _badRequestForValue = HttpResult<string>.BadRequest("Some {1} for {0}.", "Source", "error");
    private static readonly HttpResult<string> _badRequestForValueFromResult = HttpResult<string>.BadRequest(ValidationResult.Failure(_error));
    private static readonly HttpResult<string> _badRequestForValueFromError = HttpResult<string>.BadRequest(_error);
    private static readonly HttpResult<string> _badRequestForValueFromErrors = HttpResult<string>.BadRequest(new[] { _error });
    private static readonly HttpResult<string> _badRequestForValueWithMessageOnly = HttpResult<string>.BadRequest("Some error for Source.");
    private static readonly HttpResult<string> _badRequestForValueWithSourceOnly = HttpResult<string>.BadRequest("Some error for {0}.", "Source");
    private static readonly HttpResult<string> _badRequestForValueWithOtherError = HttpResult<string>.BadRequest("Other {1} for {0}.", "Source", "error");
    private static readonly HttpResult<string> _badRequestForValueWithOtherSource = HttpResult<string>.BadRequest("Some {1} for {0}.", "OtherSource", "error");
    private static readonly HttpResult<string> _badRequestForValueWithOtherData = HttpResult<string>.BadRequest("Some {1} for {0}.", "Source", "other error");

    private class TestDataForPropertiesWithValue : TheoryData<HttpResult<string>, bool, bool, bool, bool, bool, bool>
    {
        public TestDataForPropertiesWithValue()
        {
            Add(_badRequestForValue,  true, false, false, false, false, false);
            Add(_okWithValue,          false, true, false, false, false, false);
            Add(_notFoundForValue,     false, false, true, false, false, false);
            Add(_conflictForValue,     false, false, false, true, false, false);
            Add(_createdWithValue,     false, false, false, false, true, false);
            Add(_unauthorizedForValue, false, false, false, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForPropertiesWithValue))]
    public void Properties_WithValue_ShouldReturnAsExpected(HttpResult<string> subject, bool isBadRequest, bool isOk, bool isNotFound, bool isConflict, bool isCreated, bool isUnauthorized)
    {
        // Assert
        subject.IsBadRequest.Should().Be(isBadRequest);
        subject.IsOk.Should().Be(isOk);
        subject.IsNotFound.Should().Be(isNotFound);
        subject.IsConflict.Should().Be(isConflict);
        subject.IsCreated.Should().Be(isCreated);
        subject.IsUnauthorized.Should().Be(isUnauthorized);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ReturnsOk()
    {
        // Act
        HttpResult<string> subject = "Value";

        // Assert
        subject.Value.Should().Be("Value");
        subject.IsOk.Should().BeTrue();
    }

    [Fact]
    public void AddOperator_WithValueAndWithoutError_ReturnsBadRequest()
    {
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
    public void AddOperator_WithValueAndWithError_ReturnsBadRequest()
    {
        // Arrange
        var result = HttpResult<string>.Ok("Value");

        // Act
        result += _error;
        result += new ValidationError[] { new("Some error 3."), new("Some error 4.") };

        // Assert
        result.IsOk.Should().BeFalse();
        result.IsBadRequest.Should().BeTrue();
        result.Value.Should().Be("Value");
        result.ValidationErrors.Should().HaveCount(3);
    }

    [Fact]
    public void MapTo_WithoutError_ReturnsOk()
    {
        // Arrange
        var subject = HttpResult<string>.Ok("42");

        // Act
        var result = subject.Map(int.Parse);

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeTrue();
    }

    [Fact]
    public void MapTo_FromNotFound_ReturnsOk()
    {
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
    public void MapTo_WithError_ReturnsBadRequest()
    {
        // Arrange
        var subject = HttpResult<string>.BadRequest("Some error {0}.", "Source");

        // Act
        var result = subject.Map(int.Parse);

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeFalse();
    }

    private class TestDataForEqualityWithValue : TheoryData<HttpResult<string>, HttpResult<string>?, bool>
    {
        public TestDataForEqualityWithValue()
        {
            Add(_okWithValue, null, false);
            Add(_okWithValue, _okWithValue, true);
            Add(_okWithValue, _okWithOtherValue, false);
            Add(_okWithValue, _createdWithValue, false);
            Add(_okWithValue, _notFoundForValue, false);
            Add(_okWithValue, _unauthorizedForValue, false);
            Add(_okWithValue, _conflictForValue, false);
            Add(_okWithValue, _badRequestForValue, false);
            Add(_createdWithValue, null, false);
            Add(_createdWithValue, _okWithValue, false);
            Add(_createdWithValue, _createdWithValue, true);
            Add(_createdWithValue, _createdWithOtherValue, false);
            Add(_createdWithValue, _notFoundForValue, false);
            Add(_createdWithValue, _unauthorizedForValue, false);
            Add(_createdWithValue, _conflictForValue, false);
            Add(_createdWithValue, _badRequestForValue, false);
            Add(_notFoundForValue, null, false);
            Add(_notFoundForValue, _okWithValue, false);
            Add(_notFoundForValue, _createdWithValue, false);
            Add(_notFoundForValue, _notFoundForValue, true);
            Add(_notFoundForValue, _unauthorizedForValue, false);
            Add(_notFoundForValue, _conflictForValue, false);
            Add(_notFoundForValue, _badRequestForValue, false);
            Add(_unauthorizedForValue, null, false);
            Add(_unauthorizedForValue, _okWithValue, false);
            Add(_unauthorizedForValue, _createdWithValue, false);
            Add(_unauthorizedForValue, _notFoundForValue, false);
            Add(_unauthorizedForValue, _unauthorizedForValue, true);
            Add(_unauthorizedForValue, _conflictForValue, false);
            Add(_unauthorizedForValue, _badRequestForValue, false);
            Add(_conflictForValue, null, false);
            Add(_conflictForValue, _okWithValue, false);
            Add(_conflictForValue, _createdWithValue, false);
            Add(_conflictForValue, _notFoundForValue, false);
            Add(_conflictForValue, _unauthorizedForValue, false);
            Add(_conflictForValue, _conflictForValue, true);
            Add(_conflictForValue, _conflictForOtherValue, false);
            Add(_conflictForValue, _badRequestForValue, false);
            Add(_badRequestForValue, null, false);
            Add(_badRequestForValue, _okWithValue, false);
            Add(_badRequestForValue, _createdWithValue, false);
            Add(_badRequestForValue, _notFoundForValue, false);
            Add(_badRequestForValue, _unauthorizedForValue, false);
            Add(_badRequestForValue, _conflictForValue, false);
            Add(_badRequestForValue, _badRequestForValue, true);
            Add(_badRequestForValue, _badRequestForValueFromResult, true);
            Add(_badRequestForValue, _badRequestForValueFromError, true);
            Add(_badRequestForValue, _badRequestForValueFromErrors, true);
            Add(_badRequestForValue, _badRequestForValueWithSourceOnly, true);
            Add(_badRequestForValue, _badRequestForValueWithMessageOnly, false);
            Add(_badRequestForValue, _badRequestForValueWithSourceOnly, true);
            Add(_badRequestForValue, _badRequestForValueWithOtherError, false);
            Add(_badRequestForValue, _badRequestForValueWithOtherSource, false);
            Add(_badRequestForValue, _badRequestForValueWithOtherData, false);
        }
    }

    [Fact]
    public void ImplicitConversion_ToValidationResult_WithValue_ReturnsFailure()
    {
        // Act
        ValidationResult result = _badRequestForValue;

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromListOfValidationError_WithValue_ReturnsFailure()
    {
        // Act
        HttpResult<string> result = new List<IValidationError> { new ValidationError("Some error {0}.", "Source") };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(TestDataForEqualityWithValue))]
    public void Equals_WithValue_ReturnsAsExpected(HttpResult<string> subject, HttpResult<string>? other, bool expectedResult)
    {
        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEqualityWithValue))]
    public void NotEquals_WithValue_ReturnsAsExpected(HttpResult<string> subject, HttpResult<string>? other, bool expectedResult)
    {
        // Act
        var result = subject != other;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_WithValue_DifferentiatesAsExpected()
    {
        var expectedResult = new HashSet<HttpResult<string>> {
            _okWithValue,
            _okWithOtherValue,
            _badRequestForValue,
            _badRequestForValueWithOtherError,
        };

        // Act
        var result = new HashSet<HttpResult<string>> {
            HttpResult<string>.Ok("Value"),
            HttpResult<string>.Ok("Other"),
            _okWithValue,
            _okWithValue,
            _badRequestForValue,
            _badRequestForValue,
            _badRequestForValueWithSourceOnly,
            _badRequestForValueWithOtherError,
            _badRequestForValueWithSourceOnly,
            _okWithOtherValue,
            _badRequestForValueWithOtherError,
            _okWithValue,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
