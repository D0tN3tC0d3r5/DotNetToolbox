namespace DotNetToolbox.Results;

public record Result : ResultBase {
    protected Result(IEnumerable<ValidationError>? errors = null)
        : base(errors) {
    }

    public bool IsSuccess => HasNoIssues;
    public bool IsInvalid => !HasException && HasErrors;
    public bool IsFaulty => HasException;

    public static Result Success() => new();
    public static Result Invalid(string message) => Invalid(string.Empty, message);
    public static Result Invalid(string source, string message) => Invalid(new ValidationError(source, message));
    public static Result Invalid(ResultBase result) => new(result.Errors);
    public static Result Exception(string message) => Exception(new Exception(message));
    public static Result Exception(Exception exception) => new(exception);

    public static Task<Result> SuccessTask() => Task.FromResult(Success());
    public static Task<Result> InvalidTask(string message) => InvalidTask(string.Empty, message);
    public static Task<Result> InvalidTask(string source, string message) => InvalidTask(new ValidationError(source, message));
    public static Task<Result> InvalidTask(Result result) => Task.FromResult(Invalid(result));
    public static Task<Result> ExceptionTask(string message) => ExceptionTask(new Exception(message));
    public static Task<Result> ExceptionTask(Exception exception) => Task.FromResult(Exception(exception));

    public static implicit operator Result(string error) => new((ValidationError)error);
    public static implicit operator Result(Exception exception) => new((ValidationError)exception);
    public static implicit operator Result(ValidationError error) => new([error]);
    public static implicit operator Result(List<ValidationError> errors) => new([.. errors]);
    public static implicit operator Result(HashSet<ValidationError> errors) => new([.. errors]);
    public static implicit operator Result(ValidationError[] errors) => new(errors.AsEnumerable());

    public static Result operator +(Result left, Result right)
        => new(left.Errors.Union(right.Errors));

    public virtual bool Equals(Result? other)
        => other is not null
        && Errors.SequenceEqual(other.Errors);

    public override int GetHashCode()
        => Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine);

    public static Result<TValue> Success<TValue>(TValue? value) => new(value);
    public static Result<TValue> Invalid<TValue>(TValue? value, string message) => Invalid(value, string.Empty, message);
    public static Result<TValue> Invalid<TValue>(TValue? value, string source, string message) => Invalid(value, new ValidationError(source, message));
    public static Result<TValue> Invalid<TValue>(TValue? value, Result result) => new(value, result.Errors);
    public static Result<TValue> Exception<TValue>(TValue? value, string message) => Exception(value, new Exception(message));
    public static Result<TValue> Exception<TValue>(TValue? value, Exception exception) => new(value, [(ValidationError)exception]);

    public static Task<Result<TValue>> SuccessTask<TValue>(TValue? value) => Task.FromResult(Success(value));
    public static Task<Result<TValue>> InvalidTask<TValue>(TValue? value, string message) => InvalidTask(value, string.Empty, message);
    public static Task<Result<TValue>> InvalidTask<TValue>(TValue? value, string source, string message) => InvalidTask(value, new ValidationError(source, message));
    public static Task<Result<TValue>> InvalidTask<TValue>(TValue? value, Result result) => Task.FromResult(Invalid(value, result));
    public static Task<Result<TValue>> ExceptionTask<TValue>(TValue? value, string message) => ExceptionTask(value, new Exception(message));
    public static Task<Result<TValue>> ExceptionTask<TValue>(TValue? value, Exception exception) => Task.FromResult(Exception(value, exception));
}

public record Result<TValue> : Result, IResult<TValue> {
    internal Result(TValue? value = default, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator Result<TValue>(TValue? value)
        => new(value);

    public static Result<TValue> operator +(Result<TValue> left, Result right)
        => new(left.Value, left.Errors.Union(right.Errors));

    public Result<TNewValue> MapTo<TNewValue>(Func<TValue?, TNewValue?> map)
        => new(map(Value), Errors);

    public virtual bool Equals(Result<TValue>? other)
        => base.Equals(other)
        && Equals(Value, other.Value);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value);
}
