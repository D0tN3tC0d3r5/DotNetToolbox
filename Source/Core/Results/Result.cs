namespace DotNetToolbox.Results;

public record Result : ResultBase {
    protected Result(Exception exception)
        : base(exception) {
    }

    protected Result(IEnumerable<ValidationError>? errors = null)
        : base(errors) {
    }

    public bool IsFaulty => HasException;
    public bool IsInvalid => !HasException && HasErrors;
    public bool IsSuccess => !HasException && !HasErrors;

    public static Result Success() => new();
    public static Result Invalid(string message) => Invalid(string.Empty, message);
    public static Result Invalid(string source, string message) => Invalid(new ValidationError(message, source));
    public static Result Invalid(ResultBase result) => new(result.Errors);
    public static Result Exception(string message) => Exception(new Exception(message));
    public static Result Exception(Exception exception) => new(exception);

    public static Task<Result> SuccessTask() => Task.FromResult(Success());
    public static Task<Result> InvalidTask(string message) => InvalidTask(string.Empty, message);
    public static Task<Result> InvalidTask(string source, string message) => InvalidTask(new ValidationError(message, source));
    public static Task<Result> InvalidTask(Result result) => Task.FromResult(Invalid(result));
    public static Task<Result> ExceptionTask(string message) => ExceptionTask(new Exception(message));
    public static Task<Result> ExceptionTask(Exception exception) => Task.FromResult(Exception(exception));

    public static implicit operator Result(string error) => new((ValidationError)error);
    public static implicit operator Result(Exception exception) => new(exception);
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
    public static Result<TValue> Invalid<TValue>(TValue? value, string message, string? source = null) => Invalid(value, new ValidationError(message, source));
    public static Result<TValue> Invalid<TValue>(TValue? value, Result result) => new(value, result.Errors);
    public static Result<TValue> Exception<TValue>(string message) => Exception<TValue>(new Exception(message));
    public static Result<TValue> Exception<TValue>(Exception exception) => new(exception);

    public static Task<Result<TValue>> SuccessTask<TValue>(TValue? value) => Task.FromResult(Success(value));
    public static Task<Result<TValue>> InvalidTask<TValue>(TValue? value, string message, string? source = null) => InvalidTask(value, new ValidationError(message, source));
    public static Task<Result<TValue>> InvalidTask<TValue>(TValue? value, Result result) => Task.FromResult(Invalid(value, result));
    public static Task<Result<TValue>> ExceptionTask<TValue>(string message) => ExceptionTask<TValue>(new Exception(message));
    public static Task<Result<TValue>> ExceptionTask<TValue>(Exception exception) => Task.FromResult(Exception<TValue>(exception));
}

public record Result<TValue> : Result, IResult<TValue> {
    internal Result(Exception exception)
        : base(exception) {
    }

    internal Result(TValue? value = default, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator Result<TValue>(TValue? value)
        => new(value);

    public static Result<TValue> operator +(Result<TValue> left, Result right)
        => new(left.Value, left.Errors.Union(right.Errors));

    public Result<TNewValue> MapTo<TNewValue>(Func<TValue?, TNewValue?> map) {
        try {
            return HasException
                ? Exception<TNewValue>(InnerException)
                : new(map(Value), Errors);
        }
        catch (Exception ex) {
            return Exception<TNewValue>(ex);
        }
    }

    public virtual bool Equals(Result<TValue>? other)
        => base.Equals(other)
        && Equals(Value, other.Value);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value);
}
