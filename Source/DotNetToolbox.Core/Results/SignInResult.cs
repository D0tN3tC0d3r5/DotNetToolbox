using static System.Ensure;

namespace System.Results;

public sealed record SignInResult : Result {
    private const string _invalidInvalidSignInCreation =
        """
        To create an invalid result assigned the errors directly.
        i.e. SingInResult result = new ValidationError(...);
        """;
    private const string _invalidSuccessfulSignInCreation =
        """
        To make a successful result assigned the token to it.
        i.e. SingInResult result = "[Token]";
        """;

    private SignInResultType _type;

    private SignInResult(SignInResultType type, string? token = null, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        _type = HasErrors ? SignInResultType.Invalid : type;
        Token = IsSuccess ? IsNotNull(token) : null;
    }

    public string? Token { get; private set; }

    public bool IsLocked => _type is SignInResultType.Locked;
    public bool IsBlocked => _type is SignInResultType.Blocked;
    public bool IsFailure => _type is SignInResultType.Failed;
    public bool RequiresConfirmation => _type is SignInResultType.ConfirmationRequired;
    public bool RequiresTwoFactor => _type is SignInResultType.TwoFactorRequired;
    public override bool IsSuccess => _type is SignInResultType.Success;

    public static SignInResult ConfirmationRequired(string token)
        => new(SignInResultType.ConfirmationRequired, token);
    public static SignInResult TwoFactorRequired(string token)
        => new(SignInResultType.TwoFactorRequired, token);
    public static SignInResult Success(string token)
        => new(SignInResultType.Success, token);
    public static new SignInResult Invalid(string message, string source, params object?[] args)
        => new(SignInResultType.Invalid, null, new ValidationError[] { new(message, source, args) });
    public static SignInResult Invalid(Result result)
        => new(SignInResultType.Invalid, null, result.Errors);

    public static SignInResult Blocked() => new(SignInResultType.Blocked);
    public static SignInResult Locked() => new(SignInResultType.Locked);
    public static SignInResult Failure() => new(SignInResultType.Failed);

    public static implicit operator SignInResult(List<ValidationError> errors)
        => new(SignInResultType.Invalid, null, errors);
    public static implicit operator SignInResult(ValidationError[] errors)
        => new(SignInResultType.Invalid, null, errors);
    public static implicit operator SignInResult(ValidationError error)
        => new(SignInResultType.Invalid, null, new[] { error }.AsEnumerable());
    public static implicit operator SignInResult(string token)
        => new(SignInResultType.Success, token);
    public static implicit operator SignInResult(SignInResultType resultType)
        => resultType switch {
            SignInResultType.Invalid => throw new InvalidCastException(_invalidInvalidSignInCreation),
            SignInResultType.Success or SignInResultType.TwoFactorRequired => throw new InvalidCastException(_invalidSuccessfulSignInCreation),
            _ => new(resultType),
        };

    public static SignInResult operator +(SignInResult left, Result right) {
        left.Errors.Merge(right.Errors.Distinct());
        left._type = left.IsInvalid ? SignInResultType.Invalid : left._type;
        if (left.HasErrors)
            left.Token = null;
        return left;
    }

    public static bool operator ==(SignInResult left, SignInResultType right) => left._type == right;
    public static bool operator !=(SignInResult left, SignInResultType right) => left._type != right;
}
