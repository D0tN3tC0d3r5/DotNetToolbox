using static DotNetToolbox.Results.SignInResult;

namespace DotNetToolbox.Results;

public class SignInResultTests {
    private static readonly SignInResult _invalid = new ValidationError("Source", "Some error.");
    private static readonly SignInResult _invalidWithSameError = new ValidationError("Source", "Some error.");
    private static readonly SignInResult _invalidWithOtherError = new ValidationError("Source", "Other error.");
    private static readonly SignInResult _locked = Locked();
    private static readonly SignInResult _blocked = Blocked();
    private static readonly SignInResult _failure = Failure();
    private static readonly SignInResult _requiresConfirmation = ConfirmationRequired("SomeToken");
    private static readonly SignInResult _requires2Factor = TwoFactorRequired("SomeToken");
    private static readonly SignInResult _success = Success("SomeToken");
    private static readonly SignInResult _successWithSameToken = Success("SomeToken");
    private static readonly SignInResult _successWithOtherToken = Success("OtherToken");

    [Fact]
    public void Success_CreatesResult() {
        // Arrange & Act
        var result = Success("SomeToken");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Invalid_WithMessageOnly_CreatesResult() {
        // Arrange & Act
        var result = Invalid("Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Invalid_WithSourceAndMessage_CreatesResult() {
        // Arrange & Act
        var result = Invalid("Field1", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new[] {
            new ValidationError("Field1", "Some error."),
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
    public void ImplicitConversion_FromValidationError_ReturnsFailure() {
        // Act
        var result = (SignInResult)new ValidationError("Source", "Some error.");

        // Assert
        result.IsInvalid.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ReturnsFailure() {
        // Act
        SignInResult result = new[] { new ValidationError("Source", "Some error."), };

        // Assert
        result.IsInvalid.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorList_ReturnsFailure() {
        // Act
        SignInResult result = new List<ValidationError> { new("Source", "Some error."), };

        // Assert
        result.IsInvalid.Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromToken_ReturnsSuccess() {
        // Act
        SignInResult result = "SomeToken";

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    private class TestDataForProperties : TheoryData<SignInResult, bool, bool, bool, bool, bool, bool, bool> {
        public TestDataForProperties() {
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
    public void Properties_ShouldReturnAsExpected(SignInResult subject, bool isInvalid, bool isBlocked, bool isLocked, bool isFailure, bool confirmationRequired, bool twoFactorRequired, bool isSuccess) {
        // Assert
        subject.IsInvalid.Should().Be(isInvalid);
        subject.IsLocked.Should().Be(isLocked);
        subject.IsBlocked.Should().Be(isBlocked);
        subject.IsFailure.Should().Be(isFailure);
        subject.RequiresConfirmation.Should().Be(confirmationRequired);
        subject.RequiresTwoFactor.Should().Be(twoFactorRequired);
        subject.IsSuccess.Should().Be(isSuccess);
    }

    private class TestDataForEquality : TheoryData<SignInResult, SignInResultType, bool> {
        public TestDataForEquality() {
            Add(_success, SignInResultType.Success, true);
            Add(_success, Failed, false);
            Add(_success, SignInResultType.Locked, false);
            Add(_success, SignInResultType.Blocked, false);
            Add(_success, SignInResultType.Invalid, false);
            Add(_failure, SignInResultType.Success, false);
            Add(_failure, Failed, true);
            Add(_failure, SignInResultType.Locked, false);
            Add(_failure, SignInResultType.Blocked, false);
            Add(_failure, SignInResultType.Invalid, false);
            Add(_locked, SignInResultType.Success, false);
            Add(_locked, Failed, false);
            Add(_locked, SignInResultType.Locked, true);
            Add(_locked, SignInResultType.Blocked, false);
            Add(_locked, SignInResultType.Invalid, false);
            Add(_blocked, SignInResultType.Success, false);
            Add(_blocked, Failed, false);
            Add(_blocked, SignInResultType.Locked, false);
            Add(_blocked, SignInResultType.Blocked, true);
            Add(_blocked, SignInResultType.Invalid, false);
            Add(_invalid, SignInResultType.Success, false);
            Add(_invalid, Failed, false);
            Add(_invalid, SignInResultType.Locked, false);
            Add(_invalid, SignInResultType.Blocked, false);
            Add(_invalid, SignInResultType.Invalid, true);
        }
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void Equals_ReturnsAsExpected(SignInResult subject, SignInResultType type, bool expectedResult) {
        // Act
        var result = subject == type;

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [ClassData(typeof(TestDataForEquality))]
    public void NotEquals_ReturnsAsExpected(SignInResult subject, SignInResultType type, bool expectedResult) {
        // Act
        var result = subject != type;

        // Assert
        result.Should().Be(!expectedResult);
    }

    [Fact]
    public void GetHashCode_DifferentiatesAsExpected() {
        var expectedResult = new HashSet<SignInResult> {
            _success,
            _successWithOtherToken,
            _requires2Factor,
            _locked,
            _blocked,
            _failure,
            _invalid,
            _invalidWithOtherError,
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
            _invalidWithSameError,
            _invalidWithOtherError,
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void AddOperator_WithoutError_ReturnsDoesNothing() {
        // Arrange
        var result = Success("SomeToken");

        // Act
        result += Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsInvalid.Should().BeFalse();
        result.Token.Should().Be("SomeToken");
    }

    [Fact]
    public void AddOperator_WithError_ReturnsInvalid() {
        // Arrange
        var result = Success("SomeToken");

        // Act
        result += new ValidationError("Source", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Token.Should().BeNull();
    }

    [Fact]
    public void AddOperator_WithOtherError_ReturnsBothErrors() {
        // Arrange
        var result = Invalid("Source", "Some error.");

        // Act
        result += new ValidationError("Source", "Other error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void AddOperator_WithSameError_ReturnsOnlyOneError() {
        // Arrange
        var result = Invalid("Source", "Some error.");

        // Act
        result += new ValidationError("Source", "Some error.");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }
}
