namespace System.Results;

public class Result : ResultBase {
    protected Result(IEnumerable<ValidationError>? errors = null) {
        Errors = errors is null
            ? []
            : DoesNotHaveNulls(errors).ToHashSet();
    }

    public bool IsSuccess => !HasErrors;
    public bool IsInvalid => HasErrors;

    public static Result Success()
        => new();
    public static Result Invalid([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(string.Empty, message, args);
    public static Result Invalid(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(new ValidationError(source, message, args));
    public static Result Invalid(Result result)
        => new(result.Errors);

    public static implicit operator Result(List<ValidationError> errors)
        => new(errors.AsEnumerable());
    public static implicit operator Result(ValidationError[] errors)
        => new(errors.AsEnumerable());
    public static implicit operator Result(ValidationError error)
        => new(new[] { error, }.AsEnumerable());

    public static Result operator +(Result left, ResultBase right) 
        => (Result)((ResultBase)left + right);

    public bool Equals([NotNullWhen(true)] Result? other)
        => base.Equals(other);

    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Invalid<TValue>(TValue value, string message, string source, params object[] args) => new(value, new ValidationError[] { new(source, message, args), });
}

public class Result<TResult> : Result {
    internal Result(TResult value, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Value = IsNotNull(value);
    }

    public TResult Value { get; init; }

    public static implicit operator Result<TResult>(TResult value) => new(value);

    public static Result<TResult> operator +(Result<TResult> left, Result right)
        => (Result<TResult>)((Result)left + right);

    public bool Equals([NotNullWhen(true)] Result<TResult>? other)
        => base.Equals(other) && (Value?.Equals(other.Value) ?? other.Value is null);

    public Result<TOutput> MapTo<TOutput>(Func<TResult, TOutput> map)
        => new(map(Value), Errors);
}
