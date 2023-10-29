using System.Validation;
using static System.Ensure;

namespace System.Results;

public record HttpResult : Result {
    protected HttpResult(HttpResultType type, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Type = type;
    }

    public HttpResultType Type { get; protected set; }

    public override bool IsSuccess => !HasErrors && (Type is HttpResultType.Ok or HttpResultType.Created);
    public override bool IsInvalid => Type is HttpResultType.BadRequest or HttpResultType.Unauthorized or HttpResultType.NotFound or HttpResultType.Conflict;

    public bool IsOk => !HasErrors && Type is HttpResultType.Ok;
    public bool WasCreated => !HasErrors && Type is HttpResultType.Created;

    public bool IsBadRequest => HasErrors || Type is HttpResultType.BadRequest;
    public bool IsUnauthorized => Type is HttpResultType.Unauthorized;
    public bool WasNotFound => Type is HttpResultType.NotFound;
    public bool HasConflict => Type is HttpResultType.Conflict;

    public static HttpResult Ok() => new(HttpResultType.Ok);
    public static HttpResult Created() => new(HttpResultType.Created);

    public static HttpResult BadRequest(string message, string source, params object?[] args)
        => new(HttpResultType.BadRequest, new ValidationError[] { new(message, source, args) });
    public static HttpResult Unauthorized() => new(HttpResultType.Unauthorized);
    public static HttpResult NotFound() => new(HttpResultType.NotFound);
    public static HttpResult Conflict() => new(HttpResultType.Conflict);

    public static implicit operator HttpResult(List<ValidationError> errors)
        => new(HttpResultType.BadRequest, IsNotNullAndDoesNotHaveNull(errors));
    public static implicit operator HttpResult(ValidationError[] errors)
        => new(HttpResultType.BadRequest, IsNotNullAndDoesNotHaveNull(errors));
    public static implicit operator HttpResult(ValidationError error)
        => new(HttpResultType.BadRequest, new[] { error }.AsEnumerable());

    public static HttpResult operator +(HttpResult left, Result right) {
        left.Errors.Merge(right.Errors.Distinct());
        left.Type = left.IsInvalid ? HttpResultType.BadRequest : left.Type;
        return left;
    }

    public static HttpResult<TValue> Ok<TValue>(TValue value)
        => new(HttpResultType.Ok, IsNotNull(value));
    public static HttpResult<TValue> Created<TValue>(TValue value)
        => new(HttpResultType.Created, IsNotNull(value));

    public static HttpResult<TValue> BadRequest<TValue>(TValue value, string message, string source, params object?[] args)
        => new(HttpResultType.BadRequest, IsNotNull(value), new ValidationError[] { new(message, source, args) });
    public static HttpResult<TValue> Unauthorized<TValue>()
        => new(HttpResultType.Unauthorized);
    public static HttpResult<TValue> NotFound<TValue>()
        => new(HttpResultType.NotFound);
    public static HttpResult<TValue> Conflict<TValue>(TValue value)
        => new(HttpResultType.Conflict, IsNotNull(value));
}

public record HttpResult<TResult> : HttpResult {
    public HttpResult(HttpResultType type, TResult? value = default, IEnumerable<ValidationError>? errors = null)
        : base(type, errors) {
        Value = value;
    }

    public TResult? Value { get; }

    public static implicit operator HttpResult<TResult>(TResult value)
        => new(HttpResultType.Ok, IsNotNull(value));
    public static implicit operator HttpResult<TResult>(Result<TResult> result)
        => new(result.IsInvalid ? HttpResultType.BadRequest : HttpResultType.Ok, result.Value, result.Errors);

    public static HttpResult<TResult> operator +(HttpResult<TResult> left, Result right) {
        left.Errors.Merge(right.Errors.Distinct());
        left.Type = left.IsBadRequest ? HttpResultType.BadRequest : left.Type;
        return left;
    }

    public HttpResult<TOutput> MapTo<TOutput>(Func<TResult, TOutput> map)
        => Value is null
            ? NotFound<TOutput>()
            : new(Type, map(Value), Errors);
}
