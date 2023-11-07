namespace System.Results;

public class HttpResult : ResultBase {
    protected HttpResult(HttpResultType type, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Type = HasErrors ? HttpResultType.BadRequest : type;
    }

    protected HttpResultType Type { get; }

    public bool IsSuccess => !HasErrors && Type is HttpResultType.Ok or HttpResultType.Created;

    public bool IsOk => !HasErrors && Type is HttpResultType.Ok;
    public bool WasCreated => !HasErrors && Type is HttpResultType.Created;

    public bool IsBadRequest => HasErrors;
    public bool IsUnauthorized => !HasErrors && Type is HttpResultType.Unauthorized;
    public bool WasNotFound => !HasErrors && Type is HttpResultType.NotFound;
    public bool HasConflict => !HasErrors && Type is HttpResultType.Conflict;

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

    public static implicit operator HttpResult(List<ValidationError> errors)
        => new(HttpResultType.BadRequest, DoesNotHaveNulls(errors));
    public static implicit operator HttpResult(ValidationError[] errors)
        => new(HttpResultType.BadRequest, DoesNotHaveNulls(errors));
    public static implicit operator HttpResult(ValidationError error)
        => new(HttpResultType.BadRequest, new[] { error, }.AsEnumerable());

    public static HttpResult operator +(HttpResult left, Result right)
        => new(left.Type, left.Errors.Union(right.Errors));

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

    public static bool operator ==(HttpResult left, HttpResultType right)
        => left.Type == right;
    public static bool operator !=(HttpResult left, HttpResultType right)
        => left.Type != right;
    public static bool operator ==(HttpResult left, HttpResult? right)
        => left.Equals(right);
    public static bool operator !=(HttpResult left, HttpResult? right)
        => !left.Equals(right);
    public override bool Equals([NotNullWhen(true)] object? obj)
        => base.Equals(obj)
        || obj is HttpResult r && Equals(r);
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Type);

    private bool Equals(HttpResult? other)
        => base.Equals(other)
        && Type == other.Type;
}

public class HttpResult<TValue>(HttpResultType type, TValue? value = default, IEnumerable<ValidationError>? errors = null)
    : HttpResult(type, errors) {
    public TValue? Value { get; init; } = value;

    public static implicit operator HttpResult<TValue>(TValue value)
        => new(HttpResultType.Ok, IsNotNull(value));
    public static implicit operator HttpResult<TValue>(Result<TValue> result)
        => new(result.IsInvalid ? HttpResultType.BadRequest : HttpResultType.Ok, result.Value, result.Errors);

    public static HttpResult<TValue> operator +(HttpResult<TValue> left, Result right)
        => new(left.Type, left.Value, left.Errors.Union(right.Errors));

    public static bool operator ==(HttpResult<TValue> left, HttpResultType type)
        => left.Type == type;
    public static bool operator !=(HttpResult<TValue> left, HttpResultType type)
        => left.Type != type;
    public static bool operator ==(HttpResult<TValue> left, HttpResult<TValue>? right)
        => left.Equals(right);
    public static bool operator !=(HttpResult<TValue> left, HttpResult<TValue>? right)
        => !left.Equals(right);
    public static bool operator ==(HttpResult<TValue> left, TValue value)
        => left.IsSuccess && (left.Value?.Equals(value) ?? false);
    public static bool operator !=(HttpResult<TValue> left, TValue value)
        => !left.IsSuccess || !(left.Value?.Equals(value) ?? false);
    public override bool Equals([NotNullWhen(true)] object? obj)
        => base.Equals(obj)
        || obj is HttpResult<TValue> r && Equals(r);
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Type, Value);

    private bool Equals(HttpResult<TValue>? other)
        => base.Equals(other)
        && (Value?.Equals(other.Value) ?? other.Value is null);

    public HttpResult<TOutput> MapTo<TOutput>(Func<TValue, TOutput> map)
        => new(Type, Value is null ? default : map(Value), Errors);
}
