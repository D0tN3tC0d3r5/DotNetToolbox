namespace System.Results;

public record HttpResult
    : Result<HttpResult, HttpResultType>
    , IHttpResult
    , ICreateHttpResults<HttpResult>
    , IResultOperators<HttpResult> {
    private HttpResult(HttpResultType type, IEnumerable<IValidationError>? errors = null)
        : base(HttpResultType.BadRequest, type, errors) {
    }

    public bool IsBadRequest => HasErrors;
    public bool IsOk => !HasErrors && Type is HttpResultType.Ok;
    public bool IsCreated => !HasErrors && Type is HttpResultType.Created;
    public bool IsUnauthorized => !HasErrors && Type is HttpResultType.Unauthorized;
    public bool IsNotFound => !HasErrors && Type is HttpResultType.NotFound;
    public bool IsConflict => !HasErrors && Type is HttpResultType.Conflict;

    public static HttpResult BadRequest([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new ValidationError(message, args);
    public static HttpResult BadRequest(IValidationResult result)
        => (ValidationResult)result;
    public static HttpResult BadRequest(IEnumerable<IValidationError> errors)
        => errors.ToArray();
    public static HttpResult BadRequest(IValidationError error)
        => (ValidationError)error;

    public static HttpResult Ok() => new(HttpResultType.Ok);
    public static HttpResult Created() => new(HttpResultType.Created);

    public static HttpResult Unauthorized() => new(HttpResultType.Unauthorized);
    public static HttpResult NotFound() => new(HttpResultType.NotFound);
    public static HttpResult Conflict() => new(HttpResultType.Conflict);

    public static implicit operator HttpResult(Dictionary<string, string[]> errors)
        => errors.SelectMany(i => i.Value.Select(msg => new ValidationError(msg, i.Key))).ToArray();
    public static implicit operator HttpResult(List<IValidationError> errors) => errors.ToArray();
    public static implicit operator HttpResult(ValidationError error) => new[] { error };
    public static implicit operator HttpResult(IValidationError[] errors) => (ValidationResult)errors;
    public static implicit operator HttpResult(ValidationResult result)
        => new(HttpResultType.BadRequest, IsNotNullOrEmpty(result.ValidationErrors));

    public static HttpResult operator +(HttpResult left, IValidationResult right)
        => new(left.Type, left.ValidationErrors.Merge(right.ValidationErrors));
    public static HttpResult operator +(HttpResult left, IEnumerable<IValidationError> errors)
        => new(left.Type, left.ValidationErrors.Merge(errors));
    public static HttpResult operator +(HttpResult left, IValidationError error)
        => new(left.Type, left.ValidationErrors.Merge(error));

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
    private HttpResult(HttpResultType type, TValue? value = default, IEnumerable<IValidationError>? errors = null)
        : base(HttpResultType.BadRequest, type, errors) {
        Value = value;
    }

    public TValue? Value { get; }

    public bool IsOk => !HasErrors && Type is HttpResultType.Ok;
    public bool IsCreated => !HasErrors && Type is HttpResultType.Created;
    public bool IsBadRequest => ValidationErrors.Count != 0;
    public bool IsUnauthorized => Type is HttpResultType.Unauthorized;
    public bool IsNotFound => Type is HttpResultType.NotFound;
    public bool IsConflict => Type is HttpResultType.Conflict;

    public static implicit operator HttpResult<TValue>(TValue value)
        => new(HttpResultType.Ok, IsNotNull(value));
    public static implicit operator HttpResult<TValue>(List<IValidationError> errors) => errors.ToArray();
    public static implicit operator HttpResult<TValue>(ValidationError error) => new[] { error };
    public static implicit operator HttpResult<TValue>(IValidationError[] errors) => (ValidationResult)errors;
    public static implicit operator HttpResult<TValue>(ValidationResult result)
        => new(HttpResultType.BadRequest, default, IsNotNullOrEmpty(result.ValidationErrors));
    public static implicit operator ValidationResult(HttpResult<TValue> result)
        => result.ValidationErrors.ToArray();

    public static HttpResult<TValue> operator +(HttpResult<TValue> left, IValidationResult right)
        => new(left.Type, left.Value, left.ValidationErrors.Merge(right.ValidationErrors));
    public static HttpResult<TValue> operator +(HttpResult<TValue> left, IEnumerable<IValidationError> errors)
        => new(left.Type, left.Value, left.ValidationErrors.Merge(errors));
    public static HttpResult<TValue> operator +(HttpResult<TValue> left, IValidationError error)
        => new(left.Type, left.Value, left.ValidationErrors.Merge(error));

    public IHttpResult<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> map)
        => new HttpResult<TNewValue>(Type, Value is null ? default : map(Value), ValidationErrors);

    public override bool Equals(HttpResult<TValue>? other)
        => base.Equals(other)
        && (Value?.Equals(other!.Value) ?? other!.Value is null);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value?.GetHashCode() ?? 0);

    public static HttpResult<TValue> BadRequest([StringSyntax(CompositeFormat)] string message, params object?[] args)
        => new ValidationError(message, args);
    public static HttpResult<TValue> BadRequest(IValidationResult result)
        => (ValidationResult)result;
    public static HttpResult<TValue> BadRequest(IEnumerable<IValidationError> errors)
        => errors.ToArray();
    public static HttpResult<TValue> BadRequest(IValidationError error)
        => (ValidationError)error;

    public static HttpResult<TValue> Ok(TValue value) => new(HttpResultType.Ok, IsNotNull(value));
    public static HttpResult<TValue> Created(TValue value) => new(HttpResultType.Created, IsNotNull(value));
    public static HttpResult<TValue> Unauthorized() => new(HttpResultType.Unauthorized);
    public static HttpResult<TValue> NotFound() => new(HttpResultType.NotFound);
    public static HttpResult<TValue> Conflict(TValue? value) => new(HttpResultType.Conflict, value);
}
