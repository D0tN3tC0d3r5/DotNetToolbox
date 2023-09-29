namespace System.Results;

public class SignInResultTests
{
    private static readonly ValidationError _error = new("Some {1} for {0}.", "Source", "error");

    private static readonly SignInResult _locked = SignInResult.Locked();
    private static readonly SignInResult _blocked = SignInResult.Blocked();
    private static readonly SignInResult _failure = SignInResult.Failure();
    private static readonly SignInResult _requiresConfirmation = SignInResult.ConfirmationRequired("SomeToken");
    private static readonly SignInResult _requires2Factor = SignInResult.TwoFactorRequired("SomeToken");
    private static readonly SignInResult _success = SignInResult.Success("SomeToken");
    private static readonly SignInResult _successWithSameToken = SignInResult.Success("SomeToken");
    private static readonly SignInResult _successWithOtherToken = SignInResult.Success("OtherToken");
    private static readonly SignInResult _invalid = SignInResult.Invalid("Some {1} for {0}.", "Source", "error");
    private static readonly SignInResult _invalidFromResult = SignInResult.Invalid(ValidationResult.Failure(_error));
    private static readonly SignInResult _invalidFromError = SignInResult.Invalid(_error);
    private static readonly SignInResult _invalidFromErrors = SignInResult.Invalid(new[] { _error });
    private static readonly SignInResult _invalidWithMessageOnly = SignInResult.Invalid("Some error for Source.");
    private static readonly SignInResult _invalidWithSourceOnly = SignInResult.Invalid("Some error for {0}.", "Source");
    private static readonly SignInResult _invalidWithOtherSource = SignInResult.Invalid("Some {1} for {0}.", "OtherSource", "error");
    private static readonly SignInResult _invalidWithOtherData = SignInResult.Invalid("Some {1} for {0}.", "Source", "other error");
    private static readonly SignInResult _invalidWithOtherMessage = SignInResult.Invalid("Other {1} for {0}.", "Source", "error");

    [Fact]
    public void ImplicitConversion_FromValidationError_ReturnsFailure()
    {
        // Act
        var result = (SignInResult)_error;

        // Assert
        result.IsInvalid.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure()
    {
        // Act
        SignInResult result = new[] { _error };

        // Assert
        result.IsInvalid.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure()
    {
        // Act
        SignInResult result = new List<IValidationError> { new ValidationError("Some error {0}.", "Source") };

        // Assert
        result.IsInvalid.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_ToValidationResult_WithValue_ReturnsFailure()
    {
        // Act
        ValidationResult result = _invalid;

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    private class TestDataForProperties : TheoryData<SignInResult, bool, bool, bool, bool, bool, bool, bool>
    {
        public TestDataForProperties()
        {
            Add(_invalid, true, false, false, false, false, false, false);
            Add(_blocked, false, true, false, false, false, false, false);
            Add(_locked, false, false, true, false, false, false, false);
            Add(_failure, false, false, false, true, false, false, false);
            Add(_requiresConfirmation, false, false, false, false, true, false, false);
            Add(_requires2Factor, false, false, false, false, false, true, false);
            Add(_success, false, false, false, false, false, false, true);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForProperties))]
    public void Properties_ShouldReturnAsExpected(SignInResult subject, bool isInvalid, bool isBlocked, bool isLocked, bool isFailure, bool confirmationRequired, bool twoFactorRequired, bool isSuccess)
    {
        // Assert
        subject.IsInvalid.Should().Be(isInvalid);
        subject.IsLocked.Should().Be(isLocked);
        subject.IsBlocked.Should().Be(isBlocked);
        subject.IsFailure.Should().Be(isFailure);
        subject.IsConfirmationRequired.Should().Be(confirmationRequired);
        subject.IsTwoFactorRequired.Should().Be(twoFactorRequired);
        subject.IsSuccess.Should().Be(isSuccess);
    }

    private class TestDataForEquality : TheoryData<SignInResult, SignInResult?, bool>
    {
        public TestDataForEquality()
        {
            Add(_success, null, false);
            Add(_success, _success, true);
            Add(_success, _failure, false);
            Add(_success, _locked, false);
            Add(_success, _blocked, false);
            Add(_success, _invalid, false);
            Add(_failure, _success, false);
            Add(_failure, _failure, true);
            Add(_failure, _locked, false);
            Add(_failure, _blocked, false);
            Add(_failure, _invalid, false);
            Add(_locked, _success, false);
            Add(_locked, _failure, false);
            Add(_locked, _locked, true);
            Add(_locked, _blocked, false);
            Add(_locked, _invalid, false);
            Add(_blocked, _success, false);
            Add(_blocked, _failure, false);
            Add(_blocked, _locked, false);
            Add(_blocked, _blocked, true);
            Add(_blocked, _invalid, false);
            Add(_invalid, _success, false);
            Add(_invalid, _failure, false);
            Add(_invalid, _locked, false);
            Add(_invalid, _blocked, false);
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
    public void Equals_ReturnsAsExpected(SignInResult subject, SignInResult? other, bool expectedResult)
    {
        // Act
        var result = subject == other;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void NotEquals_ReturnsAsExpected(SignInResult subject, SignInResult? other, bool expectedResult)
    {
        // Act
        var result = subject != other;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_DifferentiatesAsExpected()
    {
        var expectedResult = new HashSet<SignInResult> {
            _success,
            _successWithOtherToken,
            _requires2Factor,
            _locked,
            _blocked,
            _failure,
            _invalid,
            _invalidWithOtherMessage,
        };

        // Act
        var result = new HashSet<SignInResult> {
            _success,
            _success,
            _successWithSameToken,
            _successWithOtherToken,
            _requires2Factor,
            _locked,
            _blocked,
            _failure,
            _invalid,
            _invalid,
            _invalidFromError,
            _invalidWithOtherMessage,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void AddOperator_WithoutError_ReturnsDoesNothing()
    {
        // Arrange
        var result = SignInResult.Success("SomeToken");

        // Act
        result += ValidationResult.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Token.Should().Be("SomeToken");
    }

    [Fact]
    public void AddOperator_WithInvalid_ReturnsInvalid()
    {
        // Arrange
        var result = SignInResult.Success("SomeToken");

        // Act
        result += ValidationResult.Failure("Some {1} for {0}.", "Source", "error");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().ContainSingle();
        result.Token.Should().BeNull();
    }

    [Fact]
    public void AddOperator_WithError_ReturnsInvalid()
    {
        // Arrange
        var result = SignInResult.Success("SomeToken");

        // Act
        result += _error;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().ContainSingle();
        result.Token.Should().BeNull();
    }

    [Fact]
    public void AddOperator_WithOtherError_ReturnsBothErrors()
    {
        // Arrange
        var result = SignInResult.Invalid("Some error {0}.", "Source");

        // Act
        result += new ValidationError("Other {1} for {0}.", "Source", "error");
        result += new ValidationError[] { new("Some error 3."), new("Some error 4.") };
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().HaveCount(4);
    }

    [Fact]
    public void AddOperator_WithSameError_ReturnsOnlyOneError()
    {
        // Arrange
        var result = SignInResult.Invalid(_error);

        // Act
        result += _error;

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ValidationErrors.Should().ContainSingle();
    }
}
