namespace DotNetToolbox.Results;

public record HttpResult : Result {
    private HttpResult(IResult result)
        : this(HttpResultType.Ok, result.Errors, result.Exception) {
    }

    protected HttpResult(HttpResultType type, IEnumerable<ValidationError>? errors = default, Exception? exception = default)
        : base(errors, exception) {
        Type = HasException ? HttpResultType.Error : HasErrors ? HttpResultType.BadRequest : type;
    }

    protected HttpResultType Type { get; init; }

    public override bool IsSuccess => Type is HttpResultType.Ok or HttpResultType.Created;
    public override bool IsInvalid => Type is HttpResultType.BadRequest or HttpResultType.Unauthorized or HttpResultType.NotFound or HttpResultType.Conflict;

    public bool IsOk => Type is HttpResultType.Ok;
    public bool WasCreated => Type is HttpResultType.Created;

    public bool IsBadRequest => Type is HttpResultType.BadRequest;
    public bool IsUnauthorized => Type is HttpResultType.Unauthorized;
    public bool WasNotFound => Type is HttpResultType.NotFound;
    public bool HasConflict => Type is HttpResultType.Conflict;

    public bool IsFaulty => Type is HttpResultType.Error;

    public static HttpResult Ok() => new(HttpResultType.Ok);
    public static HttpResult Created() => new(HttpResultType.Created);
    public static HttpResult BadRequest([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => BadRequest(string.Empty, message, args);
    public static HttpResult BadRequest(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => BadRequest(new ValidationError(source, message, args));
    public static HttpResult BadRequest(Result result)
        => new(HttpResultType.BadRequest, result.Errors);
    public static HttpResult Unauthorized() => new(HttpResultType.Unauthorized);
    public static HttpResult NotFound() => new(HttpResultType.NotFound);
    public static HttpResult Conflict() => new(HttpResultType.Conflict);
    public static new HttpResult Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Error(new Exception(string.Format(message, args)));
    public static new HttpResult Error(Exception exception)
        => new(HttpResultType.Error, exception: exception);

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

    public static HttpResult operator +(HttpResult left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(left.Type, errors, left.Exception ?? right.Exception);
    }

    public virtual bool Equals(HttpResult? other)
        => base.Equals(other)
        && Type == other.Type;

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Type);

    public static HttpResult<TValue> Ok<TValue>(TValue value)
        => new(HttpResultType.Ok, IsNotNull(value));
    public static HttpResult<TValue> Created<TValue>(TValue value)
        => new(HttpResultType.Created, IsNotNull(value));
    public static HttpResult<TValue> BadRequest<TValue>(TValue value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => BadRequest(value, string.Empty, message, args);
    public static HttpResult<TValue> BadRequest<TValue>(TValue value, string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => BadRequest(value, new ValidationError(source, message, args));
    public static HttpResult<TValue> BadRequest<TValue>(TValue value, Result result)
        => new(HttpResultType.BadRequest, value, result.Errors);
    public static HttpResult<TValue> Unauthorized<TValue>()
        => new(HttpResultType.Unauthorized);
    public static HttpResult<TValue> NotFound<TValue>()
        => new(HttpResultType.NotFound);
    public static HttpResult<TValue> Conflict<TValue>(TValue value)
        => new(HttpResultType.Conflict, IsNotNull(value));
    public static new HttpResult<TValue> Error<TValue>(TValue value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Error(value, new Exception(string.Format(message, args)));
    public static new HttpResult<TValue> Error<TValue>(TValue value, Exception exception)
        => new(HttpResultType.Error, value, exception: exception);
}

public record HttpResult<TValue> : HttpResult {
    internal HttpResult(IResult result)
        : this(HttpResultType.Ok, default, result.Errors, result.Exception) {
    }

    internal HttpResult(Result<TValue> result)
        : this(HttpResultType.Ok, result.Value, result.Errors, result.Exception) {
    }

    internal HttpResult(HttpResultType type, TValue? value = default, IEnumerable<ValidationError>? errors = default, Exception? exception = default)
        : base(type, errors, exception) {
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator HttpResult<TValue>(TValue? value) => new(value);
    public static implicit operator HttpResult<TValue>(Result<TValue> result) => new(result);

    public static HttpResult<TValue> operator +(HttpResult<TValue> left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(left.Type, left.Value, errors, left.Exception ?? right.Exception);
    }

    public HttpResult<TOutput> MapTo<TOutput>(Func<TValue?, TOutput?> map)
        => Type is HttpResultType.NotFound
               ? NotFound<TOutput>()
               : new(Type, map(Value), Errors);

    public virtual bool Equals(HttpResult<TValue>? other)
        => base.Equals(other)
        && Equals(Value, other.Value);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value);
}
