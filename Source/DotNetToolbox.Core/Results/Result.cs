using static System.Ensure;

namespace System.Results;

public record Result : IResult {
    protected Result(IEnumerable<ValidationError>? errors = null) {
        Errors = errors is null
            ? new()
            : IsNotNullAndDoesNotHaveNull(errors).ToList();
    }

    public IList<ValidationError> Errors { get; } = new List<ValidationError>();
    protected bool HasErrors => Errors.Count != 0;
    public virtual bool IsInvalid => HasErrors;
    public virtual bool IsSuccess => !HasErrors;

    public virtual bool Equals(Result? other)
        => other is not null
           && Errors.SequenceEqual(other.Errors);

    public override int GetHashCode()
        => Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine);

    public static Result Success() => new();
    public static Result Invalid(string message, string source, params object?[] args) => new(new ValidationError(message, source, args));

    public static implicit operator Result(List<ValidationError> errors) => new(errors.AsEnumerable());
    public static implicit operator Result(ValidationError[] errors) => new(errors.AsEnumerable());
    public static implicit operator Result(ValidationError error) => new(new[] { error }.AsEnumerable());

    public static Result operator +(Result left, Result right) {
        left.Errors.Merge(right.Errors.Distinct());
        return left;
    }

    public void EnsureIsValid(string? message = null) {
        if (HasErrors) throw new ValidationException(Errors, message);
    }

    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Invalid<TValue>(TValue value, string message, string source, params object?[] args) => new(value, new ValidationError[] { new(message, source, args) });
}

public record Result<TResult> : Result {
    internal Result(TResult value, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Value = IsNotNull(value);
    }

    public TResult Value { get; }

    public static implicit operator Result<TResult>(TResult value) => new(value);

    public static Result<TResult> operator +(Result<TResult> left, Result right) {
        left.Errors.Merge(right.Errors.Distinct());
        return left;
    }

    public Result<TOutput> MapTo<TOutput>(Func<TResult, TOutput> map)
        => new(map(Value), Errors);
}
