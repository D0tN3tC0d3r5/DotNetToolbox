namespace DotNetToolbox.Results;

public record HttpResult : ResultBase<HttpResultType> {
    private readonly HttpResultType _type = HttpResultType.Ok;

    public HttpResult() {
    }

    protected HttpResult(Exception exception)
        : base(exception) {
    }

    protected HttpResult(HttpResultType type, IEnumerable<ValidationError>? errors = default)
        : base(errors) {
        _type = type;
    }

    public override HttpResultType Type => HasException
        ? HttpResultType.Error
        : HasErrors
            ? HttpResultType.BadRequest
            : _type;

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
    public static HttpResult InternalError(string error) => InternalError(new Exception(error));
    public static HttpResult InternalError(Exception exception) => new(exception);

    public static Task<HttpResult> OkTask() => Task.FromResult(Ok());
    public static Task<HttpResult> CreatedTask() => Task.FromResult(Created());
    public static Task<HttpResult> BadRequestTask(Result result) => Task.FromResult(BadRequest(result));
    public static Task<HttpResult> UnauthorizedTask() => Task.FromResult(Unauthorized());
    public static Task<HttpResult> NotFoundTask() => Task.FromResult(NotFound());
    public static Task<HttpResult> ConflictTask() => Task.FromResult(Conflict());
    public static Task<HttpResult> InternalErrorTask(string error) => InternalErrorTask(new Exception(error));
    public static Task<HttpResult> InternalErrorTask(Exception exception) => Task.FromResult(InternalError(exception));

    public static implicit operator HttpResult(Exception exception) => new(exception);
    public static implicit operator HttpResult(string error) => (Result)error;
    public static implicit operator HttpResult(ValidationError error) => (Result)error;
    public static implicit operator HttpResult(ValidationErrors errors) => (Result)errors;
    public static implicit operator HttpResult(ValidationError[] errors) => (Result)errors;
    public static implicit operator HttpResult(List<ValidationError> errors) => (Result)errors;
    public static implicit operator HttpResult(HashSet<ValidationError> errors) => (Result)errors;
    public static implicit operator HttpResult(Result result) => new(HttpResultType.Ok, result.Errors);
    public static implicit operator ValidationErrors(HttpResult result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator ValidationError[](HttpResult result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator Exception?(HttpResult result) => result.Exception;

    public static HttpResult operator +(HttpResult left, Result right)
        => left.HasException
               ? left
               : right.HasException
                   ? new(right.Exception)
                   : new(left._type, left.Errors.Union(right.Errors));

    public static HttpResult<TValue> Ok<TValue>(TValue value) => new(HttpResultType.Ok, IsNotNull(value));
    public static HttpResult<TValue> Created<TValue>(TValue value) => new(HttpResultType.Created, IsNotNull(value));
    public static HttpResult<TValue> BadRequest<TValue>(TValue? value, Result result) => new(HttpResultType.BadRequest, value, result.Errors);
    public static HttpResult<TValue> Unauthorized<TValue>() => new(HttpResultType.Unauthorized);
    public static HttpResult<TValue> NotFound<TValue>() => new(HttpResultType.NotFound);
    public static HttpResult<TValue> Conflict<TValue>(TValue value) => new(HttpResultType.Conflict, IsNotNull(value));
    public static HttpResult<TValue> InternalError<TValue>(string error) => InternalError<TValue>(new Exception(error));
    public static HttpResult<TValue> InternalError<TValue>(Exception exception) => new(exception);

    public static Task<HttpResult<TValue>> OkTask<TValue>(TValue value) => Task.FromResult(Ok(value));
    public static Task<HttpResult<TValue>> CreatedTask<TValue>(TValue value) => Task.FromResult(Created(value));
    public static Task<HttpResult<TValue>> BadRequestTask<TValue>(TValue? value, Result result) => Task.FromResult(BadRequest(value, result));
    public static Task<HttpResult<TValue>> UnauthorizedTask<TValue>() => Task.FromResult(Unauthorized<TValue>());
    public static Task<HttpResult<TValue>> NotFoundTask<TValue>() => Task.FromResult(NotFound<TValue>());
    public static Task<HttpResult<TValue>> ConflictTask<TValue>(TValue value) => Task.FromResult(Conflict(value));
    public static Task<HttpResult<TValue>> InternalErrorTask<TValue>(string error) => InternalErrorTask<TValue>(new Exception(error));
    public static Task<HttpResult<TValue>> InternalErrorTask<TValue>(Exception exception) => Task.FromResult(InternalError<TValue>(exception));
}

public record HttpResult<TValue> : HttpResult, IResult<HttpResultType, TValue> {
    private readonly HttpResultType _type = HttpResultType.Ok;

    internal HttpResult(Exception exception)
        : base(exception) {
    }

    internal HttpResult(HttpResultType type, TValue? value = default, IEnumerable<ValidationError>? errors = default)
            : base(type, errors) {
        _type = type;
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator HttpResult<TValue>(TValue? value) => new(HttpResultType.Ok, value);
    public static implicit operator HttpResult<TValue>(Result<TValue> result) => new(HttpResultType.Ok, result.Value, result.Errors);
    public static implicit operator HttpResult<TValue>(Exception exception) => new(exception);
    public static implicit operator HttpResult<TValue>(ValidationError error) => (ValidationErrors)error;
    public static implicit operator HttpResult<TValue>(ValidationErrors errors) => new(HttpResultType.Ok, default!, errors.AsEnumerable());
    public static implicit operator HttpResult<TValue>(ValidationError[] errors) => (ValidationErrors)errors;
    public static implicit operator HttpResult<TValue>(List<ValidationError> errors) => new ValidationErrors(errors);
    public static implicit operator HttpResult<TValue>(HashSet<ValidationError> errors) => (ValidationErrors)errors;
    public static implicit operator ValidationErrors(HttpResult<TValue> result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator ValidationError[](HttpResult<TValue> result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator Exception?(HttpResult<TValue> result) => result.Exception;
    public static implicit operator TValue?(HttpResult<TValue> result) => result.Value;

    public static HttpResult<TValue> operator +(HttpResult<TValue> left, Result right)
        => left.HasException
               ? left
               : right.HasException
                   ? new(right.Exception)
                   : new(left._type, left.Value, left.Errors.Union(right.Errors));

    public HttpResult<TNewValue> MapTo<TNewValue>(Func<TValue?, TNewValue?> map) {
        try {
            return HasException
                ? InternalError<TNewValue>(Exception)
                : Type is HttpResultType.NotFound
                    ? NotFound<TNewValue>()
                    : new(Type, map(Value), Errors);
        }
        catch (Exception ex) {
            return InternalError<TNewValue>(ex);
        }
    }
}
