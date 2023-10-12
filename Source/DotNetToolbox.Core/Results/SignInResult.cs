namespace System.Results;

public sealed record SignInResult
    : Result<SignInResult, SignInResultType>
    , ISignInResult
    , ICreateSignInResults<SignInResult>
    , IResultOperators<SignInResult> {
    private SignInResult(SignInResultType type, string? token = null, IEnumerable<ValidationError>? errors = null)
        : base(SignInResultType.ValidationFailure, type, errors) {
        Token = token;
    }

    public bool IsLocked => IsValid && Type is SignInResultType.Locked;
    public bool IsBlocked => IsValid && Type is SignInResultType.Blocked;
    public bool IsFailure => IsValid && Type is SignInResultType.Failure;
    public bool IsConfirmationRequired => IsValid && Type is SignInResultType.ConfirmationRequired;
    public bool IsTwoFactorRequired => IsValid && Type is SignInResultType.TwoFactorRequired;
    public bool IsSuccess => IsValid && Type is SignInResultType.Success;
    public string? Token { get; }

    public static SignInResult Invalid([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new ValidationError(message, args);
    public static SignInResult Invalid(IValidationResult result)
        => (ValidationResult)result;
    public static SignInResult Invalid(IEnumerable<ValidationError> errors)
        => errors.ToArray();
    public static SignInResult Invalid(ValidationError error)
        => (ValidationError)error;

    public static SignInResult ConfirmationRequired(string token)
        => new(SignInResultType.ConfirmationRequired, IsNotNull(token));
    public static SignInResult TwoFactorRequired(string token)
        => new(SignInResultType.TwoFactorRequired, IsNotNull(token));
    public static SignInResult Success(string token)
        => new(SignInResultType.Success, IsNotNull(token));

    public static SignInResult Blocked() => new(SignInResultType.Blocked);
    public static SignInResult Locked() => new(SignInResultType.Locked);
    public static SignInResult Failure() => new(SignInResultType.Failure);

    public static implicit operator SignInResult(List<ValidationError> errors)
        => errors.ToArray();
    public static implicit operator SignInResult(ValidationError error)
        => new[] { error };
    public static implicit operator SignInResult(ValidationError[] errors)
        => (ValidationResult)errors;
    public static implicit operator SignInResult(ValidationResult result)
        => new(SignInResultType.ValidationFailure, null, IsNotNullOrEmpty(result.Errors));

    public static SignInResult operator +(SignInResult left, IValidationResult right)
        => new(left.Type, right.Errors.Count != 0 ? null : left.Token, left.Errors.Merge(right.Errors));
    public static SignInResult operator +(SignInResult left, IEnumerable<ValidationError> errors)
        => new(left.Type, null, left.Errors.Merge(errors));
    public static SignInResult operator +(SignInResult left, ValidationError error)
        => new(left.Type, null, left.Errors.Merge(error));

    public override bool Equals(SignInResult? other)
        => base.Equals(other)
           && (Token?.Equals(other!.Token) ?? other!.Token is null);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Token?.GetHashCode() ?? 0);
}
