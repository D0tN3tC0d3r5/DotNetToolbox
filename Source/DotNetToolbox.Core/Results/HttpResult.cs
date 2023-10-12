namespace System.Results;

public record HttpResult
    : Result<HttpResult, HttpResultType>
    , IHttpResult
    , ICreateHttpResults<HttpResult>
    , IResultOperators<HttpResult> {
    private HttpResult(HttpResultType type, IEnumerable<ValidationError>? errors = null)
        : base(HttpResultType.BadRequest, type, errors) {
    }

    public bool IsBadRequest => IsInvalid;
    public bool IsOk => IsValid && Type is HttpResultType.Ok;
    public bool IsCreated => IsValid && Type is HttpResultType.Created;
    public bool IsUnauthorized => IsValid && Type is HttpResultType.Unauthorized;
    public bool IsNotFound => IsValid && Type is HttpResultType.NotFound;
    public bool IsConflict => IsValid && Type is HttpResultType.Conflict;

    public static HttpResult BadRequest([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new ValidationError(message, args);
    public static HttpResult BadRequest(IValidationResult result)
        => (ValidationResult)result;
    public static HttpResult BadRequest(IEnumerable<ValidationError> errors)
        => errors.ToArray();
    public static HttpResult BadRequest(ValidationError error)
        => (ValidationError)error;

    public static HttpResult Ok() => new(HttpResultType.Ok);
    public static HttpResult Created() => new(HttpResultType.Created);

    public static HttpResult Unauthorized() => new(HttpResultType.Unauthorized);
    public static HttpResult NotFound() => new(HttpResultType.NotFound);
    public static HttpResult Conflict() => new(HttpResultType.Conflict);

    public static implicit operator HttpResult(Dictionary<string, string[]> errors)
        => errors.SelectMany(i => i.Value.Select(msg => new ValidationError(msg, i.Key))).ToArray();
    public static implicit operator HttpResult(List<ValidationError> errors) => errors.ToArray();
    public static implicit operator HttpResult(ValidationError error) => new[] { error };
    public static implicit operator HttpResult(ValidationError[] errors) => (ValidationResult)errors;
    public static implicit operator HttpResult(ValidationResult result)
        => new(HttpResultType.BadRequest, IsNotNullOrEmpty(result.Errors));

    public static HttpResult operator +(HttpResult left, IValidationResult right)
        => new(left.Type, left.Errors.Merge(right.Errors));
    public static HttpResult operator +(HttpResult left, IEnumerable<ValidationError> errors)
        => new(left.Type, left.Errors.Merge(errors));
    public static HttpResult operator +(HttpResult left, ValidationError error)
        => new(left.Type, left.Errors.Merge(error));

    public override bool Equals(HttpResult? other)
        => base.Equals(other);

    public override int GetHashCode()
        => base.GetHashCode();
}

public record HttpResult<TValue>
    : Result<HttpResult<TValue>, HttpResultType>
    , IHttpResult<TValue>
    , ICreateValuedHttpResults<HttpResult<TValue>, TValue>
    , IResultOperators<HttpResult<TValue>> {
    private HttpResult(HttpResultType type, TValue? value = default, IEnumerable<ValidationError>? errors = null)
        : base(HttpResultType.BadRequest, type, errors) {
        Value = value;
    }

    public TValue? Value { get; }

    public bool IsBadRequest => IsInvalid;
    public bool IsOk => IsValid && Type is HttpResultType.Ok;
    public bool IsCreated => IsValid && Type is HttpResultType.Created;
    public bool IsUnauthorized => IsValid && Type is HttpResultType.Unauthorized;
    public bool IsNotFound => IsValid && Type is HttpResultType.NotFound;
    public bool IsConflict => IsValid && Type is HttpResultType.Conflict;

    public static implicit operator HttpResult<TValue>(TValue value)
        => new(HttpResultType.Ok, IsNotNull(value));
    public static implicit operator HttpResult<TValue>(List<ValidationError> errors) => errors.ToArray();
    public static implicit operator HttpResult<TValue>(ValidationError error) => new[] { error };
    public static implicit operator HttpResult<TValue>(ValidationError[] errors) => (ValidationResult)errors;
    public static implicit operator HttpResult<TValue>(ValidationResult result)
        => new(HttpResultType.BadRequest, default, IsNotNullOrEmpty(result.Errors));
    public static implicit operator ValidationResult(HttpResult<TValue> result)
        => result.Errors.ToArray();

    public static HttpResult<TValue> operator +(HttpResult<TValue> left, IValidationResult right)
        => new(left.Type, left.Value, left.Errors.Merge(right.Errors));
    public static HttpResult<TValue> operator +(HttpResult<TValue> left, IEnumerable<ValidationError> errors)
        => new(left.Type, left.Value, left.Errors.Merge(errors));
    public static HttpResult<TValue> operator +(HttpResult<TValue> left, ValidationError error)
        => new(left.Type, left.Value, left.Errors.Merge(error));

    public IHttpResult<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> map)
        => new HttpResult<TNewValue>(Type, Value is null ? default : map(Value), Errors);

    public override bool Equals(HttpResult<TValue>? other)
        => base.Equals(other)
        && (Value?.Equals(other!.Value) ?? other!.Value is null);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value?.GetHashCode() ?? 0);

    public static HttpResult<TValue> BadRequest([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new ValidationError(message, args);
    public static HttpResult<TValue> BadRequest(IValidationResult result)
        => (ValidationResult)result;
    public static HttpResult<TValue> BadRequest(IEnumerable<ValidationError> errors)
        => errors.ToArray();
    public static HttpResult<TValue> BadRequest(ValidationError error)
        => (ValidationError)error;

    public static HttpResult<TValue> Ok(TValue value) => new(HttpResultType.Ok, IsNotNull(value));
    public static HttpResult<TValue> Created(TValue value) => new(HttpResultType.Created, IsNotNull(value));
    public static HttpResult<TValue> Unauthorized() => new(HttpResultType.Unauthorized);
    public static HttpResult<TValue> NotFound() => new(HttpResultType.NotFound);
    public static HttpResult<TValue> Conflict(TValue? value) => new(HttpResultType.Conflict, value);
}
