using static DotNetToolbox.Results.HttpResult;

namespace DotNetToolbox.Results;

public class HttpResultTests {
    private static readonly HttpResult _ok = Ok();
    private static readonly HttpResult _created = Created();
    private static readonly HttpResult _unauthorized = Unauthorized();
    private static readonly HttpResult _notFound = NotFound();
    private static readonly HttpResult _conflict = Conflict();
    private static readonly HttpResult _badRequest = BadRequest(new ValidationError("Some error.", "Source"));
    private static readonly HttpResult _badRequestWithSameError = BadRequest(new ValidationError("Some error.", "Source"));
    private static readonly HttpResult _badRequestWithOtherError = BadRequest(new ValidationError("Other error.", "Source"));
    private static readonly HttpResult _failure = InternalError(new Exception("Some error."));

    private static readonly HttpResult<string> _okWithValue = Ok("Value");
    private static readonly HttpResult<string> _createdWithValue = Created("Value");
    private static readonly HttpResult<string> _unauthorizedWithValue = Unauthorized<string>();
    private static readonly HttpResult<string> _notFoundWithValue = NotFound<string>();
    private static readonly HttpResult<string> _conflictWithValue = Conflict("Value");
    private static readonly HttpResult<string> _badRequestWithValue = BadRequest("Value", new ValidationError("Some error.", "Source"));
    private static readonly HttpResult<string> _failureWithValue = InternalError<string>(new Exception("Some error."));

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure() {
        // Act
        HttpResult result = new ValidationError("Some error.", nameof(result));

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure() {
        // Act
        HttpResult result = new[] { new ValidationError("Some error.", nameof(result)) };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure() {
        // Act
        HttpResult result = new List<ValidationError> { new("Some error.", nameof(result)) };

        // Assert
        result.IsOk.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<HttpResult, bool, bool, bool, bool, bool, bool> {
        public TestDataForProperties() {
            Add(_badRequest, true, false, false, false, false, false);
            Add(_failure, false, false, false, false, false, false);
            Add(_ok, false, true, false, false, false, false);
            Add(_notFound, false, false, true, false, false, false);
            Add(_conflict, false, false, false, true, false, false);
            Add(_created, false, false, false, false, true, false);
            Add(_unauthorized, false, false, false, false, false, true);
            Add(_badRequestWithValue, true, false, false, false, false, false);
            Add(_failureWithValue, false, false, false, false, false, false);
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
            Add(_badRequest, _badRequestWithOtherError, false);
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
            _badRequestWithOtherError,
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
            _badRequestWithOtherError,
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
        var result = BadRequest(new ValidationError("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void BadRequest_WithSourceAndMessage_CreatesResult() {
        // Arrange & Act
        var result = BadRequest(new ValidationError("Some error.", "Field1"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new[] {
            new ValidationError("Some error.", "Field1"),
        });
    }

    [Fact]
    public void BadRequest_WithResult_CreatesResult() {
        // Arrange & Act
        var result = BadRequest(Result.Invalid("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void AddOperator_WithoutError_ReturnsBadRequest() {
        // Arrange
        var result = Ok();

        // Act
        result += Result.Success();

        // Assert
        result.IsOk.Should().BeTrue();
        result.IsBadRequest.Should().BeFalse();
    }

    [Fact]
    public void AddOperator_WithOtherError_ReturnsBothErrors() {
        // Arrange
        var result = BadRequest(new ValidationError("Some error.", "Source"));

        // Act
        result += new ValidationError("Source", "Other error.");

        // Assert
        result.IsOk.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void AddOperator_WithSameError_ReturnsOnlyOneError() {
        // Arrange
        var result = BadRequest(new ValidationError("Some error.", "Source"));

        // Act
        result += new ValidationError("Some error.", "Source");

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
        result += new ValidationError("Some error.", "Source");

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
        HttpResult<string> subject = Result.Success("Value");

        // Assert
        subject.Value.Should().Be("Value");
        subject.IsOk.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromBadRequestResult_ReturnsOk() {
        // Act
        var result = Result.Invalid("Value", "Some error.", "SomeProperty");
        HttpResult<string> subject = result;

        // Assert
        subject.Value.Should().Be("Value");
        subject.IsOk.Should().BeFalse();
        subject.Errors.Should().BeEquivalentTo(result.Errors);
    }

    [Fact]
    public void AddOperator_WithValueAndWithoutError_ReturnsBadRequest() {
        // Arrange
        var result = Ok("Value");

        // Act
        result += Result.Success();

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
        result += new ValidationError("Some error.", "result");

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
        var result = subject.MapTo(s => s is null ? default : int.Parse(s));

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeTrue();
    }

    [Fact]
    public void MapTo_FromNotFound_ReturnsOk() {
        // Arrange
        var subject = NotFound<string>();

        // Act
        var result = subject.MapTo(s => s is null ? default : int.Parse(s));

        // Assert
        result.Should().BeOfType<HttpResult<int>>();
        result.IsOk.Should().BeFalse();
        result.IsBadRequest.Should().BeFalse();
        result.WasNotFound.Should().BeTrue();
    }

    [Fact]
    public void MapTo_WithError_ReturnsBadRequest() {
        // Arrange
        var subject = BadRequest("42", new ValidationError("Some error.", "Field1"));

        // Act
        var result = subject.MapTo(s => s is null ? default : int.Parse(s));

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
        var result = BadRequest(42, new ValidationError("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void BadRequestOfT_WithSourceAndMessage_CreatesResult() {
        // Arrange & Act
        var result = BadRequest(42, new ValidationError("Some error.", "Field1"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new[] {
            new ValidationError("Some error.", "Field1"),
        });
    }

    [Fact]
    public void BadRequestOfT_WithResult_CreatesResult() {
        // Arrange & Act
        var result = BadRequest(42, Result.Invalid("Some error."));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public async Task OkTask_ReturnsTaskWithSuccessCrudResult() {
        // Act
        var task = OkTask();

        // Assert
        var result = await task;
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreatedTask_ReturnsTaskWithSuccessCrudResult() {
        // Act
        var task = CreatedTask();

        // Assert
        var result = await task;
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UnauthorizedTask_ReturnsTaskWithSuccessCrudResult() {
        // Act
        var task = UnauthorizedTask();

        // Assert
        var result = await task;
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task NotFoundTask_ReturnsTaskWithNotFoundCrudResult() {
        // Act
        var task = NotFoundTask();

        // Assert
        var result = await task;
        result.WasNotFound.Should().BeTrue();
    }

    [Fact]
    public async Task ConflictTask_ReturnsTaskWithConflictCrudResult() {
        // Act
        var task = ConflictTask();

        // Assert
        var result = await task;
        result.HasConflict.Should().BeTrue();
    }

    [Fact]
    public async Task BadRequestTask_ReturnsTaskWithInvalidCrudResult() {
        // Act
        var task = BadRequestTask("Some error.");

        // Assert
        var result = await task;
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public async Task OkTaskTValue_ReturnsTaskWithSuccessCrudResult() {
        // Act
        var task = OkTask("Test value");

        // Assert
        var result = await task;
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Test value");
    }

    [Fact]
    public async Task CreatedTaskTValue_ReturnsTaskWithSuccessCrudResult() {
        // Act
        var task = CreatedTask("Test value");

        // Assert
        var result = await task;
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Test value");
    }

    [Fact]
    public async Task UnauthorizedTaskTValue_ReturnsTaskWithSuccessCrudResult() {
        // Act
        var task = UnauthorizedTask<string>();

        // Assert
        var result = await task;
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void MapTo_WhenMappingFunctionThrows_ResultsInErrorCrudResult() {
        // Arrange
        var result = Ok("42");
        static int MappingFunction(string _) => throw new InvalidOperationException();

        // Act
        var mappedResult = result.MapTo(MappingFunction!);

        // Assert
        mappedResult.Should().BeOfType<HttpResult<int>>();
        mappedResult.HasException.Should().BeTrue();
        mappedResult.Exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public void MapTo_WhenTypeIsNotFound_ReturnsNotFoundCrudResult() {
        // Arrange
        var result = NotFound<string>();

        // Act
        var mappedResult = result.MapTo(s => s!.Length);

        // Assert
        mappedResult.Should().BeOfType<HttpResult<int>>();
        mappedResult.WasNotFound.Should().BeTrue();
    }

    [Fact]
    public void Error_WithString_CreatesResultWithException() {
        // Arrange
        const string errorMessage = "Error message";

        // Act
        var result = InternalError(errorMessage);

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeOfType<Exception>();
        result.Exception!.Message.Should().Be(errorMessage);
    }

    [Fact]
    public void Error_WithException_CreatesResultWithException() {
        // Arrange
        var exception = new Exception("Error message");

        // Act
        var result = InternalError(exception);

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeSameAs(exception);
    }

    [Fact]
    public async Task NotFoundTaskTValue_ReturnsTaskWithNotFoundHttpResult() {
        // Act
        var task = NotFoundTask<int>();

        // Assert
        var result = await task;
        result.WasNotFound.Should().BeTrue();
        result.Value.Should().Be(default);
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorToHttpResultTValue_ReturnsBadRequest() {
        // Act
        HttpResult<int> result = new ValidationError("Some error.", nameof(result));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Value.Should().Be(default);
    }

    [Fact]
    public void AdditionOperator_CombiningHttpResultWithError_ReturnsResultWithError() {
        // Arrange
        var result = Ok();
        var error = new ValidationError("Error message", "Property");

        // Act
        var combinedResult = result + error;

        // Assert
        combinedResult.HasErrors.Should().BeTrue();
        combinedResult.Errors.Should().ContainSingle(e => e.Message == "Error message" && e.Source == "Property");
    }

    // ... similar tests for other operator overloads ...

    [Fact]
    public void MapTo_WithException_ReturnsHttpResultTNewValueWithSameException() {
        // Arrange
        var exception = new Exception("Error message");
        var subject =  InternalError<int>(exception);

        // Act
        var result = subject.MapTo(value => value * 2);

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeSameAs(exception);
    }

    [Fact]
    public void ValueProperty_IsSetCorrectly() {
        // Arrange
        const string expectedValue = "Test value";

        // Act
        var result = Ok(expectedValue);

        // Assert
        result.Value.Should().Be(expectedValue);
    }

    // ... similar tests for other factory methods and implicit conversions that set Value ...

    [Fact]
    public void EnsureIsSuccess_WhenHasErrors_ThrowsValidationException() {
        // Arrange
        var result = BadRequest(new ValidationError("Error message"));

        // Act & Assert
        var action = () => result.EnsureIsSuccess();

        action.Should().Throw<ValidationException>().WithMessage(ValidationException.DefaultMessage);
    }

    [Fact]
    public void EnsureIsSuccess_WhenHasException_ThrowsValidationException() {
        // Arrange
        var exception = new Exception("Original exception");
        var result = InternalError(exception);

        // Act & Assert
        var action = () => result.EnsureIsSuccess();

        action.Should().Throw<ValidationException>().WithInnerException<Exception>().WithMessage("Original exception");
    }

    [Fact]
    public void TypeProperty_WhenHasException_ReturnsError() {
        // Arrange
        var exception = new Exception("Error message");

        // Act
        var result = InternalError(exception);

        // Assert
        result.Type.Should().Be(HttpResultType.Error);
    }

    [Fact]
    public async Task InternalErrorTask_WithString_ReturnsTaskWithErrorHttpResult() {
        // Arrange
        const string errorMessage = "Error message";

        // Act
        var task = InternalErrorTask(errorMessage);
        var result = await task;

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeOfType<Exception>();
        result.Exception!.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task InternalErrorTask_WithException_ReturnsTaskWithErrorHttpResult() {
        // Arrange
        var exception = new Exception("Error message");

        // Act
        var task = InternalErrorTask(exception);
        var result = await task;

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeSameAs(exception);
    }

    [Fact]
    public void ImplicitConversion_FromExceptionToHttpResult_CreatesResultWithException() {
        // Arrange
        var exception = new Exception("Error message");

        // Act
        HttpResult result = exception;

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeSameAs(exception);
    }

    [Fact]
    public void ImplicitConversion_FromStringToHttpResult_CreatesResultWithException() {
        // Act
        HttpResult result = "Error message";

        // Assert
        result.HasException.Should().BeFalse();
        result.HasErrors.Should().BeTrue();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorsToHttpResult_CreatesResultWithErrors() {
        // Arrange
        var errors = new ValidationErrors {
            new ValidationError("Error message", "Property"),
        };

        // Act
        HttpResult result = errors;

        // Assert
        result.HasErrors.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitConversion_FromHashSetValidationErrorToHttpResult_CreatesResultWithErrors() {
        // Arrange
        var errors = new HashSet<ValidationError> {
            new("Error message", "Property"),
        };

        // Act
        HttpResult result = errors;

        // Assert
        result.HasErrors.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitConversion_FromHttpResultToValidationErrorArray_ReturnsErrors() {
        // Arrange
        var errors = new[] { new ValidationError("Error message", "Property") };

        // Act
        ValidationError[] errorArray = BadRequest(errors);
        Exception? resultException = BadRequest(errors);

        // Assert
        errorArray.Should().BeEquivalentTo(errors);
        resultException.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_FromHttpResultToException_ReturnsException() {
        // Arrange
        var exception = new Exception("Error message");

        // Act
        ValidationError[] errorArray = InternalError(exception);
        Exception? resultException = InternalError(exception);

        // Assert
        errorArray.Should().BeEmpty();
        resultException.Should().BeSameAs(exception);
    }

    [Fact]
    public void ErrorOfT_WithString_CreatesResultWithException() {
        // Arrange
        const string errorMessage = "Error message";

        // Act
        var result =  InternalError<int>(errorMessage);

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeOfType<Exception>();
        result.Exception!.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task ConflictTaskTValue_ReturnsTaskWithConflictHttpResult() {
        // Arrange
        const string value = "Test value";

        // Act
        var task = ConflictTask(value);
        var result = await task;

        // Assert
        result.HasConflict.Should().BeTrue();
        result.Value.Should().Be(value);
    }

    [Fact]
    public async Task BadRequestTaskTValue_ReturnsTaskWithInvalidHttpResult() {
        // Arrange
        const string value = "Test value";
        var errors = new[] { new ValidationError("Error message", "Property") };
        var invalidResult = Result.Invalid(errors);

        // Act
        var task = BadRequestTask(value, invalidResult);
        var result = await task;

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Value.Should().Be(value);
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public async Task InternalErrorTaskTValue_WithString_ReturnsTaskWithErrorHttpResult() {
        // Arrange
        const string errorMessage = "Error message";

        // Act
        var task = InternalErrorTask<int>(errorMessage);
        var result = await task;

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeOfType<Exception>();
        result.Exception!.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task InternalErrorTaskTValue_WithException_ReturnsTaskWithErrorHttpResult() {
        // Arrange
        var exception = new Exception("Error message");

        // Act
        var task = InternalErrorTask<int>(exception);
        var result = await task;

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeSameAs(exception);
    }

    [Fact]
    public void AdditionOperator_LeftHasException_ReturnsLeft() {
        // Arrange
        var left = InternalError(new Exception("Left exception"));
        var right = Result.Success();

        // Act
        var result = left + right;

        // Assert
        result.Should().BeSameAs(left);
        result.HasException.Should().BeTrue();
        result.Exception!.Message.Should().Be("Left exception");
    }

    [Fact]
    public void AdditionOperator_RightHasException_ReturnsNewHttpResultWithRightException() {
        // Arrange
        var left = Ok();
        var rightException = new Exception("Right exception");
        var right = Result.Error(rightException);

        // Act
        var result = left + right;

        // Assert
        result.Should().NotBeSameAs(left);
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeSameAs(rightException);
    }

    [Fact]
    public void AdditionOperator_NeitherHasException_ReturnsCombinedErrors() {
        // Arrange
        var leftErrors = new[] { new ValidationError("Left error", "LeftProperty") };
        var rightErrors = new[] { new ValidationError("Right error", "RightProperty") };
        var left = BadRequest(leftErrors);
        var right = Result.Invalid(rightErrors);

        // Act
        var result = left + right;

        // Assert
        result.HasException.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().Contain(leftErrors.Union(rightErrors));
    }

    [Fact]
    public void AdditionOperator_NeitherHasExceptionAndDuplicateErrors_ReturnsDistinctErrors() {
        // Arrange
        var sharedError = new ValidationError("Shared error", "SharedProperty");
        var leftErrors = new[] { sharedError };
        var rightErrors = new[] { sharedError };
        var left = BadRequest(leftErrors);
        var right = Result.Invalid(rightErrors);

        // Act
        var result = left + right;

        // Assert
        result.HasException.Should().BeFalse();
        result.Errors.Should().ContainSingle(); // Only one instance of the shared error
        result.Errors.Should().Contain(sharedError);
    }

    [Fact]
    public void ImplicitConversion_FromExceptionToHttpResultTValue_CreatesResultWithException() {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        HttpResult<string> result = exception;

        // Assert
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeSameAs(exception);
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArrayToHttpResultTValue_CreatesResultWithErrors() {
        // Arrange
        var errors = new[] { new ValidationError("Error 1", "Property1"), new ValidationError("Error 2", "Property2") };

        // Act
        HttpResult<string> result = errors;

        // Assert
        result.HasErrors.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorListToHttpResultTValue_CreatesResultWithErrors() {
        // Arrange
        var errors = new List<ValidationError> { new("Error 1", "Property1"), new("Error 2", "Property2") };

        // Act
        HttpResult<string> result = errors;

        // Assert
        result.HasErrors.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorHashSetToHttpResultTValue_CreatesResultWithErrors() {
        // Arrange
        var errors = new HashSet<ValidationError> { new("Error 1", "Property1"), new("Error 2", "Property2") };

        // Act
        HttpResult<string> result = errors;

        // Assert
        result.HasErrors.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void ImplicitConversion_FromHttpResultTValueToValidationErrorArray_ReturnsErrors() {
        // Arrange
        var errors = new[] { new ValidationError("Error 1", "Property1"), new ValidationError("Error 2", "Property2") };
        var result = BadRequest("Value", errors);

        // Act
        ValidationError[] errorArray = result;
        Exception? resultException = result;

        // Assert
        errorArray.Should().BeEquivalentTo(errors);
        resultException.Should().BeNull();
    }

    [Fact]
    public void ImplicitConversion_FromHttpResultTValueToException_ReturnsException() {
        // Arrange
        var exception = new Exception("Test exception");
        var result = InternalError<string>(exception);

        // Act
        ValidationError[] errorArray = result;
        Exception? resultException = result;

        // Assert
        errorArray.Should().BeEmpty();
        resultException.Should().BeSameAs(exception);
    }

    [Fact]
    public void ImplicitConversion_FromHttpResultTValueToTValue_ReturnsValue() {
        // Arrange
        const string value = "Test value";
        var result = Ok(value);

        // Act
        string? resultValue = result;

        // Assert
        resultValue.Should().Be(value);
    }

    [Fact]
    public void AdditionOperator_CombiningHttpResultTValueWithResult_ReturnsCombinedErrors() {
        // Arrange
        const string value = "Test value";
        var leftErrors = new[] { new ValidationError("Left error", "LeftProperty") };
        var rightErrors = new[] { new ValidationError("Right error", "RightProperty") };
        var left = BadRequest(value, leftErrors);
        var right = Result.Invalid(rightErrors);

        // Act
        var result = left + right;

        // Assert
        result.HasException.Should().BeFalse();
        result.Value.Should().Be(value);
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().Contain(leftErrors.Union(rightErrors));
    }

    [Fact]
    public void AdditionOperator_CombiningHttpResultTValueWithResultWhenLeftHasException_ReturnsLeft() {
        // Arrange
        var leftException = new Exception("Left exception");
        var left = InternalError<string>(leftException);
        var right = Result.Success();

        // Act
        var result = left + right;

        // Assert
        result.Should().BeSameAs(left);
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeSameAs(leftException);
    }

    [Fact]
    public void AdditionOperator_CombiningHttpResultTValueWithResultWhenRightHasException_ReturnsNewHttpResultTValueWithRightException() {
        // Arrange
        const string value = "Test value";
        var left = Ok(value);
        var rightException = new Exception("Right exception");
        var right = Result.Error(rightException);

        // Act
        var result = left + right;

        // Assert
        result.Should().NotBeSameAs(left);
        result.HasException.Should().BeTrue();
        result.Exception.Should().BeSameAs(rightException);
    }
}
