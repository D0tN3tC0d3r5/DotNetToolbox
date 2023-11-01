namespace System.Results;

public sealed record SignInResult : Result {
    private SignInResultType _type;

    private SignInResult(SignInResultType type, string? token = null, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        _type = HasErrors ? SignInResultType.Invalid : type;
        Token = IsSuccess ? IsNotNull(token) : null;
    }

    public string? Token { get; private init; }

    public bool IsLocked => _type is SignInResultType.Locked;
    public bool IsBlocked => _type is SignInResultType.Blocked;
    public bool IsFailure => _type is SignInResultType.Failed;
    public bool RequiresConfirmation => _type is SignInResultType.ConfirmationRequired;
    public bool RequiresTwoFactor => _type is SignInResultType.TwoFactorRequired;
    public override bool IsSuccess => _type is SignInResultType.Success;

    public static SignInResult Success(string token)
        => new(SignInResultType.Success, token);
    public static SignInResult ConfirmationRequired(string token)
        => new(SignInResultType.ConfirmationRequired, token);
    public static SignInResult TwoFactorRequired(string token)
        => new(SignInResultType.TwoFactorRequired, token);
    public static new SignInResult Invalid([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(string.Empty, message, args);
    public static new SignInResult Invalid(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(new ValidationError(source, message, args));
    public static new SignInResult Invalid(Result result)
        => new(SignInResultType.Invalid, null, result.Errors);

    public static SignInResult Blocked() => new(SignInResultType.Blocked);
    public static SignInResult Locked() => new(SignInResultType.Locked);
    public static SignInResult Failure() => new(SignInResultType.Failed);

    public static implicit operator SignInResult(List<ValidationError> errors)
        => new(SignInResultType.Invalid, null, errors);
    public static implicit operator SignInResult(ValidationError[] errors)
        => new(SignInResultType.Invalid, null, errors);
    public static implicit operator SignInResult(ValidationError error)
        => new(SignInResultType.Invalid, null, new[] { error, }.AsEnumerable());
    public static implicit operator SignInResult(string token)
        => new(SignInResultType.Success, token);

    public static SignInResult operator +(SignInResult left, Result right) {
        left.Errors.UnionWith(right.Errors);
        return left with {
            _type = left.IsInvalid ? SignInResultType.Invalid : left._type,
            Token = left.IsInvalid ? null : left.Token,
        };
    }

    public static bool operator ==(SignInResult left, SignInResultType right) => left._type == right;
    public static bool operator !=(SignInResult left, SignInResultType right) => left._type != right;
}
