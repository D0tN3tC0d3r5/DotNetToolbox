namespace DotNetToolbox.Results;

public record SignInResult : ResultBase<SignInResultType> {
    private readonly SignInResultType _type = SignInResultType.Success;
    private readonly string? _token;

    private SignInResult(Exception exception)
        : base(exception) {
    }

    private SignInResult(SignInResultType type, string? token = default, IEnumerable<ValidationError>? errors = default)
        : base(errors) {
        //Token = token ?? throw new ArgumentNullException(nameof(token));
        _type = type;
        _token = token;
    }

    public override SignInResultType Type => HasException
        ? SignInResultType.Error
        : HasErrors
            ? SignInResultType.Invalid
            : _type;

    public string Token => HasException || HasErrors
        ? string.Empty
        : _token ?? string.Empty;

    public bool IsLocked => Type is SignInResultType.Locked;
    public bool IsBlocked => Type is SignInResultType.Blocked;
    public bool IsFailure => Type is SignInResultType.Failed;
    public bool RequiresConfirmation => Type is SignInResultType.ConfirmationPending;
    public bool RequiresTwoFactor => Type is SignInResultType.TwoFactorRequired;
    public bool IsSuccess => Type is SignInResultType.Success;
    public bool IsInvalid => Type is SignInResultType.Invalid;

    [MemberNotNull(nameof(Token))]
    public static SignInResult Success(string token) => new(SignInResultType.Success, IsNotNull(token));
    [MemberNotNull(nameof(Token))]
    public static SignInResult ConfirmationRequired(string token) => new(SignInResultType.ConfirmationPending, IsNotNull(token));
    [MemberNotNull(nameof(Token))]
    public static SignInResult TwoFactorRequired(string token) => new(SignInResultType.TwoFactorRequired, IsNotNull(token));
    public static SignInResult InvalidRequest(Result result) => new(SignInResultType.Invalid, errors: result.Errors);
    public static SignInResult BlockedAccount() => new(SignInResultType.Blocked);
    public static SignInResult LockedAccount() => new(SignInResultType.Locked);
    public static SignInResult FailedAttempt() => new(SignInResultType.Failed);
    public static SignInResult Error(string error) => Error(new Exception(error));
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
    public static Task<SignInResult> ErrorTask(string error) => ErrorTask(new Exception(error));
    public static Task<SignInResult> ErrorTask(Exception exception) => Task.FromResult(Error(exception));

    public static implicit operator SignInResult(string token) => new(SignInResultType.Success, token);

    public static implicit operator SignInResult(Exception exception) => (Result)exception;
    public static implicit operator SignInResult(ValidationError error) => (Result)error;
    public static implicit operator SignInResult(ValidationErrors errors) => (Result)errors;
    public static implicit operator SignInResult(ValidationError[] errors) => (Result)errors;
    public static implicit operator SignInResult(List<ValidationError> errors) => (Result)errors;
    public static implicit operator SignInResult(HashSet<ValidationError> errors) => (Result)errors;
    public static implicit operator SignInResult(Result result) => new(SignInResultType.Success, errors: result.Errors);
    public static implicit operator ValidationError[](SignInResult result) => result.HasException ? [] : result.Errors.ToArray();
    public static implicit operator Exception?(SignInResult result) => result.Exception;

    public static SignInResult operator +(SignInResult left, Result right)
        => left.HasException
               ? left
               : right.HasException
                   ? new(right.Exception)
                   : new(left._type, left.Token, left.Errors.Union(right.Errors));
}
