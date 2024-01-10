namespace DotNetToolbox.Results;

public record Result : ResultBase {
    protected Result(IEnumerable<ValidationError>? errors = null, Exception? exception = null)
        : base(errors, exception) {
    }

    public bool IsSuccess => HasNoIssues;
    public bool IsInvalid => HasErrors && !HasException;

    public static Result Success() => new();
    public static Result InvalidData(string message) => InvalidData(string.Empty, message);
    public static Result InvalidData(string source, string message) => InvalidData(new ValidationError(source, message));
    public static Result InvalidData(ResultBase result) => new(result.Errors);
    public static Result Error(string message) => Error(new Exception(message));
    public static Result Error(Exception exception) => new(exception: exception);

    public static Task<Result> SuccessTask() => Task.FromResult(Success());
    public static Task<Result> InvalidDataTask(string message) => InvalidDataTask(string.Empty, message);
    public static Task<Result> InvalidDataTask(string source, string message) => InvalidDataTask(new ValidationError(source, message));
    public static Task<Result> InvalidDataTask(Result result) => Task.FromResult(InvalidData(result));
    public static Task<Result> ErrorTask(string message) => ErrorTask(new Exception(message));
    public static Task<Result> ErrorTask(Exception exception) => Task.FromResult(Error(exception));

    public static implicit operator Result(ValidationError error) => new([ error ]);
    public static implicit operator Result(List<ValidationError> errors) => new([..errors]);
    public static implicit operator Result(HashSet<ValidationError> errors) => new([..errors]);
    public static implicit operator Result(ValidationError[] errors) => new(errors.AsEnumerable());
    public static implicit operator Result(Exception exception) => new(exception: exception);

    public static Result operator +(Result left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(errors, left.Exception ?? right.Exception);
    }

    public virtual bool Equals(Result? other)
        => other is not null
        && Errors.SequenceEqual(other.Errors)
        && Equals(Exception, other.Exception);

    public override int GetHashCode()
        => HashCode.Combine(Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine), Exception);

    public static Result<TValue> Success<TValue>(TValue? value) => new(value);
    public static Result<TValue> InvalidData<TValue>(TValue? value, string message) => InvalidData(value, string.Empty, message);
    public static Result<TValue> InvalidData<TValue>(TValue? value, string source, string message) => InvalidData(value, new ValidationError(source, message));
    public static Result<TValue> InvalidData<TValue>(TValue? value, Result result) => new(value, result.Errors);
    public static Result<TValue> Error<TValue>(TValue? value, string message) => Error(value, new Exception(message));
    public static Result<TValue> Error<TValue>(TValue? value, Exception exception) => new(value, exception: exception);

    public static Task<Result<TValue>> SuccessTask<TValue>(TValue? value) => Task.FromResult(Success(value));
    public static Task<Result<TValue>> InvalidDataTask<TValue>(TValue? value, string message) => InvalidDataTask(value, string.Empty, message);
    public static Task<Result<TValue>> InvalidDataTask<TValue>(TValue? value, string source, string message) => InvalidDataTask(value, new ValidationError(source, message));
    public static Task<Result<TValue>> InvalidDataTask<TValue>(TValue? value, Result result) => Task.FromResult(InvalidData(value, result));
    public static Task<Result<TValue>> ErrorTask<TValue>(TValue? value, string message) => ErrorTask(value, new Exception(message));
    public static Task<Result<TValue>> ErrorTask<TValue>(TValue? value, Exception exception) => Task.FromResult(Error(value, exception));
}

public record Result<TValue> : Result, IResult<TValue> {
    internal Result(TValue? value = default, IEnumerable<ValidationError>? errors = null, Exception? exception = null)
        : base(errors, exception) {
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator Result<TValue>(TValue? value) => new(value);

    public static Result<TValue> operator +(Result<TValue> left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(left.Value, errors, left.Exception ?? right.Exception);
    }

    public Result<TNewValue> MapTo<TNewValue>(Func<TValue?, TNewValue?> map)
        => new(map(Value), Errors, Exception);

    public virtual bool Equals(Result<TValue>? other)
        => base.Equals(other)
        && Equals(Value, other.Value);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value);
}
