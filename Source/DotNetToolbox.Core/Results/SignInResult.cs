namespace System.Results;

public sealed class SignInResult : ResultBase {
    private SignInResultType? _type;

    private SignInResult(SignInResultType? type, string? token = null, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        _type = HasErrors ? null : type;
        Token = !IsSuccess ? null : IsNotNull(token);
    }

    public string? Token { get; private set; }

    public bool IsInvalid => HasErrors;
    public bool IsFailure => !HasErrors && _type == SignInResultType.Failed;
    public bool IsLocked => !HasErrors && _type == SignInResultType.Locked;
    public bool IsBlocked => !HasErrors && _type == SignInResultType.Blocked;
    public bool RequiresConfirmation => !HasErrors && _type == SignInResultType.ConfirmationRequired;
    public bool RequiresTwoFactor => !HasErrors && _type == SignInResultType.TwoFactorRequired;
    public bool IsSuccess => !HasErrors && _type == SignInResultType.Success;

    public static SignInResult Invalid([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(string.Empty, message, args);
    public static SignInResult Invalid(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(new ValidationError(source, message, args));
    public static SignInResult Invalid(Result result)
        => new(default, null, result.Errors);

    public static SignInResult Success(string token)
        => new(SignInResultType.Success, token);
    public static SignInResult ConfirmationRequired(string token)
        => new(SignInResultType.ConfirmationRequired, token);
    public static SignInResult TwoFactorRequired(string token)
        => new(SignInResultType.TwoFactorRequired, token);

    public static SignInResult Failure() => new(SignInResultType.Failed);
    public static SignInResult Blocked() => new(SignInResultType.Blocked);
    public static SignInResult Locked() => new(SignInResultType.Locked);

    public static implicit operator SignInResult(List<ValidationError> errors)
        => new(null, null, errors);
    public static implicit operator SignInResult(ValidationError[] errors)
        => new(null, null, errors);
    public static implicit operator SignInResult(ValidationError error)
        => new(null, null, [ error, ]);
    public static implicit operator SignInResult(string token)
        => new(SignInResultType.Success, token);

    public static SignInResult operator +(SignInResult left, Result right) {
        left.Errors.UnionWith(right.Errors);
        left._type = left.HasErrors ? default : left._type;
        left.Token = left.HasErrors ? null : left.Token;
        return left;
    }

    public static bool operator ==(SignInResult left, SignInResultType right)
        => !left.HasErrors && left._type == right;
    public static bool operator !=(SignInResult left, SignInResultType right)
        => left.HasErrors || left._type != right;

    public bool Equals(SignInResult? other)
        => base.Equals(other) && _type == other._type && Token == other.Token;
}
