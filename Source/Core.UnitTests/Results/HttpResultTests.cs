using static DotNetToolbox.Results.HttpResult;

namespace DotNetToolbox.Results;

public class HttpResultTests {
    private static readonly HttpResult _ok = Ok();
    private static readonly HttpResult _created = Created();
    private static readonly HttpResult _unauthorized = Unauthorized();
    private static readonly HttpResult _notFound = NotFound();
    private static readonly HttpResult _conflict = Conflict();
    private static readonly HttpResult _badRequest = BadRequest("Source", "Some error.");
    private static readonly HttpResult _badRequestWithSameError = new ValidationError("Source", "Some error.");
    private static readonly HttpResult _badRequestWithWithOtherError = new ValidationError("Source", "Other error.");

    private static readonly HttpResult<string> _okWithValue = Ok("Value");
    private static readonly HttpResult<string> _createdWithValue = Created("Value");
    private static readonly HttpResult<string> _unauthorizedWithValue = Unauthorized<string>();
    private static readonly HttpResult<string> _notFoundWithValue = NotFound<string>();
    private static readonly HttpResult<string> _conflictWithValue = Conflict("Value");
    private static readonly HttpResult<string> _badRequestWithValue = BadRequest("Value", "Source", "Some error.");

    [Fact]
    public void CopyConstructor_ClonesObject() {
        // Act
        var result = _ok with {
            Errors = new HashSet<ValidationError> { new("Some error.") },
        };

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure() {
        // Act
        HttpResult result = new ValidationError(nameof(result), "Some error.");

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure() {
        // Act
        HttpResult result = new[] { new ValidationError(nameof(result), "Some error.") };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure() {
        // Act
        HttpResult result = new List<ValidationError> { new(nameof(result), "Some error.") };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<HttpResult, bool, bool, bool, bool, bool, bool> {
        public TestDataForProperties() {
            Add(_badRequest, true, false, false, false, false, false);
            Add(_ok, false, true, false, false, false, false);
            Add(_notFound, false, false, true, false, false, false);
            Add(_conflict, false, false, false, true, false, false);
            Add(_created, false, false, false, false, true, false);
            Add(_unauthorized, false, false, false, false, false, true);
            Add(_badRequestWithValue, true, false, false, false, false, false);
            Add(_okWithValue, false, true, false, false, false, false);
            Add(_notFoundWithValue, false, false, true, false, false, false);
            Add(_conflictWithValue, false, false, false, true, false, false);
            Add(_createdWithValue, false, false, false, false, true, false);
            Add(_unauthorizedWithValue, false, false, false, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForProperties))]
    public void Properties_ShouldReturnAsExpected(HttpResult subject, bool isBadRequest, bool isOk, bool isNotFound, bool isConflict, bool isCreated, bool isUnauthorized) {
        // Assert
        subject.IsBadRequest.Should().Be(isBadRequest);
        subject.IsOk.Should().Be(isOk);
        subject.WasNotFound.Should().Be(isNotFound);
        subject.HasConflict.Should().Be(isConflict);
        subject.WasCreated.Should().Be(isCreated);
        subject.IsUnauthorized.Should().Be(isUnauthorized);
    }

    private class TestDataForEquality : TheoryData<HttpResult, HttpResult?, bool> {
        public TestDataForEquality() {
            Add(_ok, null, false);
            Add(_ok, _ok, true);
            Add(_ok, _notFound, false);
            Add(_ok, _conflict, false);
            Add(_ok, _badRequest, false);
            Add(_notFound, null, false);
            Add(_notFound, _ok, false);
            Add(_notFound, _notFound, true);
            Add(_notFound, _conflict, false);
            Add(_notFound, _badRequest, false);
            Add(_conflict, null, false);
            Add(_conflict, _ok, false);
            Add(_conflict, _notFound, false);
            Add(_conflict, _conflict, true);
            Add(_conflict, _badRequest, false);
            Add(_badRequest, null, false);
            Add(_badRequest, _ok, false);
            Add(_badRequest, _notFound, false);
            Add(_badRequest, _conflict, false);
            Add(_badRequest, _badRequest, true);
            Add(_badRequest, _badRequestWithSameError, true);
            Add(_badRequest, _badRequestWithWithOtherError, false);
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
            _badRequestWithWithOtherError,
        };

        // Act
        var result = new HashSet<HttpResult> {
            Ok(),
            Ok(),
            _ok,
            _ok,
            _badRequest,
            _badRequest,
            _badRequestWithSameError,
            _badRequestWithWithOtherError,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Ok_CreatesResult() {
        // Arrange & Act
        var result = Ok();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void BadRequest_WithMessageOnly_CreatesResult() {
        // Arrange & Act
        var result = BadRequest("Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void BadRequest_WithSourceAndMessage_CreatesResult() {
        // Arrange & Act
        var result = BadRequest("Field1", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new[] {
            new ValidationError("Field1", "Some error."),
        });
    }

    [Fact]
    public void BadRequest_WithResult_CreatesResult() {
        // Arrange & Act
        var result = BadRequest(Invalid("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void AddOperator_WithoutError_ReturnsBadRequest() {
        // Arrange
        var result = Ok();

        // Act
        result += Success();

        // Assert
        result.IsOk.Should().BeTrue();
        result.IsBadRequest.Should().BeFalse();
    }

    [Fact]
    public void AddOperator_WithOtherError_ReturnsBothErrors() {
        // Arrange
        var result = BadRequest("Source", "Some error.");

        // Act
        result += new ValidationError("Source", "Other error.");

        // Assert
        result.IsOk.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void AddOperator_WithSameError_ReturnsOnlyOneError() {
        // Arrange
        var result = BadRequest("Source", "Some error.");

        // Act
        result += new ValidationError("Source", "Some error.");

        // Assert
        result.IsOk.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void OfT_CopyConstructor_ClonesObject() {
        // Arrange
        var original = Ok(42);

        // Act
        var result = original with {
            Value = 7,
        };

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(7);
    }

    [Fact]
    public void AddOperator_WithError_ReturnsBadRequest() {
        // Arrange
        var result = Ok("SomeToken");

        // Act
        result += new ValidationError("Source", "Some error.");

        // Assert
        result.IsOk.Should().BeFalse();
        result.Errors.Should().ContainSingle();
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
    public void ImplicitConversion_FromOkResult_ReturnsOk() {
        // Act
        var result = Success("Value");
        HttpResult<string> subject = result;

        // Assert
        subject.Value.Should().Be(result.Value);
        subject.IsOk.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromBadRequestResult_ReturnsOk() {
        // Act
        var result = Invalid("Value", "Some error.", "SomeProperty");
        HttpResult<string> subject = result;

        // Assert
        subject.Value.Should().Be(result.Value);
        subject.IsOk.Should().BeFalse();
        subject.Errors.Should().BeEquivalentTo(result.Errors);
    }

    [Fact]
    public void AddOperator_WithValueAndWithoutError_ReturnsBadRequest() {
        // Arrange
        var result = Ok("Value");

        // Act
        result += Success();

        // Assert
        result.IsOk.Should().BeTrue();
        result.IsBadRequest.Should().BeFalse();
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void AddOperator_WithValueAndWithError_ReturnsBadRequest() {
        // Arrange
        var result = Ok("Value");

        // Act
        result += new ValidationError("result", "Some error.");

        // Assert
        result.IsOk.Should().BeFalse();
        result.IsBadRequest.Should().BeTrue();
        result.Value.Should().Be("Value");
    }

    [Fact]
    public void MapTo_WithoutError_ReturnsOk() {
        // Arrange
        var subject = Ok("42");

        // Act
        var result = subject.MapTo(int.Parse);

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeTrue();
    }

    [Fact]
    public void MapTo_FromNotFound_ReturnsOk() {
        // Arrange
        var subject = NotFound<string>();

        // Act
        var result = subject.MapTo(int.Parse);

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeFalse();
        result.IsBadRequest.Should().BeFalse();
        result.WasNotFound.Should().BeTrue();
    }

    [Fact]
    public void MapTo_WithError_ReturnsBadRequest() {
        // Arrange
        var subject = BadRequest("42", "Field", "Some error.");

        // Act
        var result = subject.MapTo(int.Parse);

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void OkOfT_CreatesResult() {
        // Arrange & Act
        var result = Ok(42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void BadRequestOfT_WithMessageOnly_CreatesResult() {
        // Arrange & Act
        var result = BadRequest(42, "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void BadRequestOfT_WithSourceAndMessage_CreatesResult() {
        // Arrange & Act
        var result = BadRequest(42, "Field1", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new[] {
            new ValidationError("Field1", "Some error."),
        });
    }

    [Fact]
    public void BadRequestOfT_WithResult_CreatesResult() {
        // Arrange & Act
        var result = BadRequest(42, Invalid("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }
}
