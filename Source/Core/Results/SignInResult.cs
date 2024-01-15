namespace DotNetToolbox.Results;

public record SignInResult : ResultBase {
    private SignInResult(SignInResultType type, string token)
        : this(type) {
        Token = token;
    }

    private SignInResult(IResult result)
        : this(SignInResultType.Success, result.Errors) {
    }

    private SignInResult(Exception exception)
        : base(exception) {
        Type = SignInResultType.Error;
    }

    private SignInResult(SignInResultType type, IEnumerable<ValidationError>? errors = default)
        : base(errors) {
        SetType(type);
    }

    internal SignInResultType Type { get; private set; }
    private void SetType(SignInResultType type)
        => Type = HasException
            ? SignInResultType.Error
            : HasErrors
                ? SignInResultType.Invalid
                : type;

    protected override void OnErrorsChanged(IReadOnlyCollection<ValidationError> errors)
        => SetType(Type);

    public string? Token { get; init; }

    public bool IsLocked => Type is SignInResultType.Locked;
    public bool IsBlocked => Type is SignInResultType.Blocked;
    public bool IsFailure => Type is SignInResultType.Failed;
    public bool RequiresConfirmation => Type is SignInResultType.ConfirmationRequired;
    public bool RequiresTwoFactor => Type is SignInResultType.TwoFactorRequired;
    public bool IsSuccess => Type is SignInResultType.Success;
    public bool IsInvalid => Type is SignInResultType.Invalid;

    [MemberNotNull(nameof(Token))]
    public static SignInResult Success(string token) => new(SignInResultType.Success, IsNotNull(token));
    [MemberNotNull(nameof(Token))]
    public static SignInResult ConfirmationRequired(string token) => new(SignInResultType.ConfirmationRequired, IsNotNull(token));
    [MemberNotNull(nameof(Token))]
    public static SignInResult TwoFactorRequired(string token) => new(SignInResultType.TwoFactorRequired, IsNotNull(token));
    public static SignInResult InvalidRequest(Result result) => new(SignInResultType.Invalid, result.Errors);
    public static SignInResult BlockedAccount() => new(SignInResultType.Blocked);
    public static SignInResult LockedAccount() => new(SignInResultType.Locked);
    public static SignInResult FailedAttempt() => new(SignInResultType.Failed);
    public static SignInResult Error(Exception exception) => new(exception);

    [MemberNotNull(nameof(Token))]
    public static Task<SignInResult> SuccessTask(string token) => Task.FromResult(Success(token));
    [MemberNotNull(nameof(Token))]
    public static Task<SignInResult> ConfirmationRequiredTask(string token) => Task.FromResult(ConfirmationRequired(token));
    [MemberNotNull(nameof(Token))]
    public static Task<SignInResult> TwoFactorRequiredTask(string token) => Task.FromResult(TwoFactorRequired(token));
    public static Task<SignInResult> InvalidTask(Result result) => Task.FromResult(InvalidRequest(result));
    public static Task<SignInResult> BlockedAccountTask() => Task.FromResult(BlockedAccount());
    public static Task<SignInResult> LockedAccountTask() => Task.FromResult(LockedAccount());
    public static Task<SignInResult> FailedAttemptTask() => Task.FromResult(FailedAttempt());
    public static Task<SignInResult> ErrorTask(Exception exception) => Task.FromResult(Error(exception));

    public static implicit operator SignInResult(List<ValidationError> errors) => new((Result)errors);
    public static implicit operator SignInResult(ValidationError[] errors) => new((Result)errors);
    public static implicit operator SignInResult(ValidationError error) => new((Result)error);
    public static implicit operator SignInResult(HashSet<ValidationError> errors) => new((Result)errors);
    public static implicit operator SignInResult(Exception exception) => new(exception);
    public static implicit operator SignInResult(Result result) => new((IResult)result);
    public static implicit operator SignInResult(string token) => new(SignInResultType.Success, token);

    public static SignInResult operator +(SignInResult left, SignInResult right) {
        var errors = left.Errors.Union(right.Errors).ToArray();
        return errors.Length > 0
            ? new(left.Errors.Count > 0 ? left.Type : right.Type, left.Errors.Union(right.Errors))
            : new(left.Type, left.Token!);
    }

    public static SignInResult operator +(SignInResult left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToArray();
        return errors.Length > 0
                   ? new(left.Errors.Count > 0 ? left.Type : SignInResultType.Invalid, left.Errors.Union(right.Errors))
                   : new(left.Type, left.Token!);
    }

    public virtual bool Equals(SignInResult? other)
        => base.Equals(other)
        && Type == other.Type;

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Type);
}
