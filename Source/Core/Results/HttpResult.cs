namespace DotNetToolbox.Results;

public record HttpResult : ResultBase {
    private HttpResult(IResult result)
        : this(HttpResultType.Ok, result.Errors) {
    }

    protected HttpResult(Exception exception)
        : base(exception) {
        Type = HttpResultType.Error;
    }

    protected HttpResult(HttpResultType type, IEnumerable<ValidationError>? errors = default)
        : base(errors) {
        SetType(type);
    }

    internal HttpResultType Type { get; private set; }
    private void SetType(HttpResultType type)
        => Type = HasException
            ? HttpResultType.Error
            : HasErrors
                ? HttpResultType.BadRequest
                : type;

    protected override void OnErrorsChanged(IReadOnlyCollection<ValidationError> errors)
        => SetType(Type);

    public bool IsSuccess => Type is HttpResultType.Ok or HttpResultType.Created;
    public bool IsInvalid => Type is HttpResultType.BadRequest or HttpResultType.Unauthorized or HttpResultType.NotFound or HttpResultType.Conflict;

    public bool IsOk => Type is HttpResultType.Ok;
    public bool WasCreated => Type is HttpResultType.Created;

    public bool IsBadRequest => Type is HttpResultType.BadRequest;
    public bool IsUnauthorized => Type is HttpResultType.Unauthorized;
    public bool WasNotFound => Type is HttpResultType.NotFound;
    public bool HasConflict => Type is HttpResultType.Conflict;

    public bool IsFaulty => Type is HttpResultType.Error;

    public static HttpResult Ok() => new(HttpResultType.Ok);
    public static HttpResult Created() => new(HttpResultType.Created);
    public static HttpResult BadRequest(Result result) => new(HttpResultType.BadRequest, result.Errors);
    public static HttpResult Unauthorized() => new(HttpResultType.Unauthorized);
    public static HttpResult NotFound() => new(HttpResultType.NotFound);
    public static HttpResult Conflict() => new(HttpResultType.Conflict);
    public static HttpResult InternalError(Exception exception) => new(exception);

    public static Task<HttpResult> OkTask() => Task.FromResult(Ok());
    public static Task<HttpResult> CreatedTask() => Task.FromResult(Created());
    public static Task<HttpResult> BadRequestTask(Result result) => Task.FromResult(BadRequest(result));
    public static Task<HttpResult> UnauthorizedTask() => Task.FromResult(Unauthorized());
    public static Task<HttpResult> NotFoundTask() => Task.FromResult(NotFound());
    public static Task<HttpResult> ConflictTask() => Task.FromResult(Conflict());
    public static Task<HttpResult> InternalErrorTask(Exception exception) => Task.FromResult(InternalError(exception));

    public static implicit operator HttpResult(ValidationError error)
        => new((Result)error);
    public static implicit operator HttpResult(List<ValidationError> errors)
        => new((Result)errors);
    public static implicit operator HttpResult(ValidationError[] errors)
        => new((Result)errors);
    public static implicit operator HttpResult(HashSet<ValidationError> errors)
        => new((Result)errors);
    public static implicit operator HttpResult(Exception exception)
        => new((Result)exception);
    public static implicit operator HttpResult(Result result)
        => new((IResult)result);

    public static HttpResult operator +(HttpResult left, HttpResult right)
        => new(right.Type, left.Errors.Union(right.Errors));

    public static HttpResult operator +(HttpResult left, Result right)
        => new(left.Type, left.Errors.Union(right.Errors));

    public virtual bool Equals(HttpResult? other)
        => base.Equals(other)
        && Type == other.Type;

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Type);

    public static HttpResult<TValue> Ok<TValue>(TValue value) => new(HttpResultType.Ok, IsNotNull(value));
    public static HttpResult<TValue> Created<TValue>(TValue value) => new(HttpResultType.Created, IsNotNull(value));
    public static HttpResult<TValue> BadRequest<TValue>(TValue? value, Result result) => new(HttpResultType.BadRequest, value, result.Errors);
    public static HttpResult<TValue> Unauthorized<TValue>() => new(HttpResultType.Unauthorized);
    public static HttpResult<TValue> NotFound<TValue>() => new(HttpResultType.NotFound);
    public static HttpResult<TValue> Conflict<TValue>(TValue value) => new(HttpResultType.Conflict, IsNotNull(value));
    public static HttpResult<TValue> InternalError<TValue>(Exception exception) => new(exception);

    public static Task<HttpResult<TValue>> OkTask<TValue>(TValue value) => Task.FromResult(Ok(value));
    public static Task<HttpResult<TValue>> CreatedTask<TValue>(TValue value) => Task.FromResult(Created(value));
    public static Task<HttpResult<TValue>> BadRequestTask<TValue>(TValue? value, Result result) => Task.FromResult(BadRequest(value, result));
    public static Task<HttpResult<TValue>> UnauthorizedTask<TValue>() => Task.FromResult(Unauthorized<TValue>());
    public static Task<HttpResult<TValue>> NotFoundTask<TValue>() => Task.FromResult(NotFound<TValue>());
    public static Task<HttpResult<TValue>> ConflictTask<TValue>(TValue value) => Task.FromResult(Conflict(value));
    public static Task<HttpResult<TValue>> InternalErrorTask<TValue>(Exception exception) => Task.FromResult(InternalError<TValue>(exception));
}

public record HttpResult<TValue> : HttpResult, IResult<TValue> {
    internal HttpResult(IResult<TValue> result)
        : this(HttpResultType.Ok, result.Value, result.Errors) {
    }

    internal HttpResult(Exception exception)
        : base(exception) {
    }

    internal HttpResult(HttpResultType type, TValue? value = default, IEnumerable<ValidationError>? errors = default)
            : base(type, errors) {
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator HttpResult<TValue>(TValue? value) => new(HttpResultType.Ok, value);
    public static implicit operator HttpResult<TValue>(Result<TValue> result) => new((IResult<TValue>)result);

    public static HttpResult<TValue> operator +(HttpResult<TValue> left, HttpResult right)
        => new(right.Type, left.Value, left.Errors.Union(right.Errors));

    public static HttpResult<TValue> operator +(HttpResult<TValue> left, Result right)
        => new(left.Type, left.Value, left.Errors.Union(right.Errors));

    public HttpResult<TNewValue> MapTo<TNewValue>(Func<TValue?, TNewValue?> map) {
        try {
            return HasException
                ? InternalError<TNewValue>(((ResultBase)this).Exception)
                : Type is HttpResultType.NotFound
                    ? NotFound<TNewValue>()
                    : new(Type, map(Value), Errors);
        }
        catch (Exception ex) {
            return InternalError<TNewValue>(ex);
        }
    }

    public virtual bool Equals(HttpResult<TValue>? other)
        => base.Equals(other)
        && Equals(Value, other.Value);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value);
}
