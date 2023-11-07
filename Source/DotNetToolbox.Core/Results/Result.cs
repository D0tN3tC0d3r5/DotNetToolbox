namespace System.Results;

public class Result : ResultBase {
    protected Result(IEnumerable<ValidationError>? errors = null)
        : base(errors) {
    }

    public bool IsSuccess => !HasErrors;
    public bool IsInvalid => HasErrors;

    public static Result Success()
        => new();
    public static Result Invalid([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(string.Empty, message, args);
    public static Result Invalid(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid((Result)new ValidationError(source, message, args));
    private static Result Invalid(IResult result)
        => new(result.Errors);

    public static implicit operator Result(List<ValidationError> errors)
        => new(errors.AsEnumerable());
    public static implicit operator Result(ValidationError[] errors)
        => new(errors.AsEnumerable());
    public static implicit operator Result(ValidationError error)
        => new(new[] { error, }.AsEnumerable());

    public static Result operator +(Result left, ResultBase right)
        => new (left.Errors.Union(right.Errors));

    public static bool operator ==(Result left, Result? right)
        => left.Equals(right);
    public static bool operator !=(Result left, Result? right)
        => !left.Equals(right);
    public override bool Equals([NotNullWhen(true)] object? obj)
        => base.Equals(obj)
        || obj is Result r && Equals(r);
    public override int GetHashCode() => base.GetHashCode();

    private bool Equals(Result? other)
        => base.Equals(other);

    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Invalid<TValue>(TValue value, string message, string source, params object[] args) => new(value, new ValidationError[] { new(source, message, args), });
}

public class Result<TValue> : Result {
    internal Result(TValue value, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Value = IsNotNull(value);
    }

    public TValue Value { get; init; }

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static Result<TValue> operator +(Result<TValue> left, Result right)
        => new(left.Value, left.Errors.Union(right.Errors));

    public static bool operator ==(Result<TValue> left, Result<TValue>? right)
        => left.Equals(right);
    public static bool operator !=(Result<TValue> left, Result<TValue>? right)
        => !left.Equals(right);
    public static bool operator ==(Result<TValue> left, TValue? right)
        => left.IsSuccess && (left.Value?.Equals(right) ?? right is null);
    public static bool operator !=(Result<TValue> left, TValue? right)
        => !left.IsSuccess || !(left.Value?.Equals(right) ?? right is null);
    public override bool Equals([NotNullWhen(true)] object? obj)
        => base.Equals(obj)
        || obj is Result<TValue> r && Equals(r);
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);

    private bool Equals([NotNullWhen(true)] Result<TValue>? other)
        => base.Equals(other)
        && (Value?.Equals(other.Value) ?? other.Value is null);

    public Result<TNewValue> MapTo<TNewValue>(Func<TValue, TNewValue> map)
        => new(map(Value), Errors);
}
