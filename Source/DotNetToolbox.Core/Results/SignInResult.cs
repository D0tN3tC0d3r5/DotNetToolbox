namespace DotNetToolbox.Results;

public sealed record SignInResult
    : Result<SignInResult, SignInResultType>
    , ISignInResult
    , ICreateSignInResults<SignInResult>
    , IResultOperators<SignInResult>
{
    private SignInResult(SignInResultType type, string? token = null, IEnumerable<IValidationError>? errors = null)
        : base(SignInResultType.ValidationFailure, type, errors)
    {
        Token = token;
    }

    public bool IsInvalid => HasErrors;
    public bool IsLocked => !HasErrors && Type == SignInResultType.Locked;
    public bool IsBlocked => !HasErrors && Type == SignInResultType.Blocked;
    public bool IsFailure => !HasErrors && Type == SignInResultType.Failure;
    public bool IsConfirmationRequired => !HasErrors && Type == SignInResultType.ConfirmationRequired;
    public bool IsTwoFactorRequired => !HasErrors && Type == SignInResultType.TwoFactorRequired;
    public bool IsSuccess => !HasErrors && Type == SignInResultType.Success;
    public string? Token { get; }

    public static SignInResult Invalid([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new ValidationError(message, args);
    public static SignInResult Invalid(IValidationResult result)
        => (ValidationResult)result;
    public static SignInResult Invalid(IEnumerable<IValidationError> errors)
        => errors.ToArray();
    public static SignInResult Invalid(IValidationError error)
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

    public static implicit operator SignInResult(List<IValidationError> errors)
        => errors.ToArray();
    public static implicit operator SignInResult(ValidationError error)
        => new [] { error };
    public static implicit operator SignInResult(IValidationError[] errors)
        => (ValidationResult)errors;
    public static implicit operator SignInResult(ValidationResult result)
        => new(SignInResultType.ValidationFailure, null, IsNotNullOrEmpty(result.ValidationErrors));

    public static SignInResult operator +(SignInResult left, IValidationResult right)
        => new(left.Type, right.ValidationErrors.Count != 0 ? null : left.Token, left.ValidationErrors.Merge(right.ValidationErrors));
    public static SignInResult operator +(SignInResult left, IEnumerable<IValidationError> errors)
        => new(left.Type, null, left.ValidationErrors.Merge(errors));
    public static SignInResult operator +(SignInResult left, IValidationError error)
        => new(left.Type, null, left.ValidationErrors.Merge(error));

    public override bool Equals(SignInResult? other)
        => base.Equals(other)
           && (Token?.Equals(other!.Token) ?? other!.Token is null);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Token?.GetHashCode() ?? 0);
}
