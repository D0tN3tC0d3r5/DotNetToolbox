namespace DotNetToolbox.Results;

public record SignInResult : Result {

    private SignInResult(string token)
        : this(SignInResultType.Success, token) {
    }

    private SignInResult(IResult result)
        : this(SignInResultType.Success, default, result.Errors, result.Exception) {
    }

    private SignInResult(SignInResultType type, string? token = default, IEnumerable<ValidationError>? errors = default, Exception? exception = default)
        : base(errors, exception) {
        Type = HasException ? SignInResultType.Error : HasErrors ? SignInResultType.Invalid : type;
        Token = HasException || HasErrors ? default : token;
    }

    internal SignInResultType Type { get; }
    public string? Token { get; init; }

    public bool IsLocked => Type is SignInResultType.Locked;
    public bool IsBlocked => Type is SignInResultType.Blocked;
    public bool IsFailure => Type is SignInResultType.Failed;
    public bool RequiresConfirmation => Type is SignInResultType.ConfirmationRequired;
    public bool RequiresTwoFactor => Type is SignInResultType.TwoFactorRequired;
    public override bool IsSuccess => Type is SignInResultType.Success;
    public override bool IsInvalid => Type is SignInResultType.Invalid;

    [MemberNotNull(nameof(Token))]
    public static SignInResult Success(string token)
        => new(SignInResultType.Success, IsNotNull(token));
    [MemberNotNull(nameof(Token))]
    public static SignInResult ConfirmationRequired(string token)
        => new(SignInResultType.ConfirmationRequired, IsNotNull(token));
    [MemberNotNull(nameof(Token))]
    public static SignInResult TwoFactorRequired(string token)
        => new(SignInResultType.TwoFactorRequired, IsNotNull(token));
    public static new SignInResult Invalid([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(string.Empty, message, args);
    public static new SignInResult Invalid(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(new ValidationError(source, message, args));
    public static new SignInResult Invalid(Result result)
        => new(SignInResultType.Invalid, default, result.Errors);
    public static SignInResult Blocked() => new(SignInResultType.Blocked);
    public static SignInResult Locked() => new(SignInResultType.Locked);
    public static SignInResult Failure() => new(SignInResultType.Failed);
    public static new SignInResult Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Error(new Exception(string.Format(message, args)));
    public static new SignInResult Error(Exception exception)
        => new(SignInResultType.Error, exception: exception);

    public static implicit operator SignInResult(List<ValidationError> errors)
        => new((Result)errors);
    public static implicit operator SignInResult(ValidationError[] errors)
        => new((Result)errors);
    public static implicit operator SignInResult(ValidationError error)
        => new((Result)error);
    public static implicit operator SignInResult(HashSet<ValidationError> errors)
        => new((Result)errors);
    public static implicit operator SignInResult(Exception exception)
        => new((Result)exception);
    public static implicit operator SignInResult(string token)
        => new(token);

    public static SignInResult operator +(SignInResult left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(left.Type, left.Token, errors, left.Exception ?? right.Exception);
    }

    public virtual bool Equals(SignInResult? other)
        => base.Equals(other)
        && Type == other.Type;

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Type);
}
