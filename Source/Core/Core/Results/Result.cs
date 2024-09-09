namespace DotNetToolbox.Results;

public record Result : ResultBase<ResultType> {
    public Result() {
    }

    protected Result(Exception exception)
        : base(exception) {
    }

    protected Result(IEnumerable<ValidationError>? errors = null)
        : base(errors) {
    }

    public override ResultType Type => HasException
                                           ? ResultType.Error
                                           : HasErrors
                                               ? ResultType.Invalid
                                               : ResultType.Success;

    public bool IsFaulty => HasException;
    public bool IsInvalid => !HasException && HasErrors;
    public bool IsSuccess => !HasException && !HasErrors;

    public void EnsureSuccess() {
        if (HasException) throw Exception;
        if (HasErrors) throw new ValidationException(Errors);
    }

    public static Result Success() => new();
    public static Result Invalid(string message) => Invalid(message, string.Empty);
    public static Result Invalid(string message, string source) => Invalid(new ValidationError(message, source));
    public static Result Invalid(ValidationError error) => new([error]);
    public static Result Invalid(Result result) => new((IEnumerable<ValidationError>)result.Errors);
    public static Result Error(string message) => Error(new Exception(message));
    public static Result Error(Exception exception) => new(exception);

    public static Task<Result> SuccessTask() => Task.FromResult(Success());
    public static Task<Result> InvalidTask(string message) => InvalidTask(message, string.Empty);
    public static Task<Result> InvalidTask(string message, string source) => InvalidTask(new ValidationError(message, source));
    public static Task<Result> InvalidTask(ValidationError result) => Task.FromResult(Invalid(result));
    public static Task<Result> InvalidTask(Result result) => Task.FromResult(Invalid(result));
    public static Task<Result> ErrorTask(string message) => ErrorTask(new Exception(message));
    public static Task<Result> ErrorTask(Exception exception)
        => Task.FromResult(Error(exception));

    public static implicit operator Result(Exception exception) => new(exception);
    public static implicit operator Result(string error) => (ValidationErrors)error;
    public static implicit operator Result(ValidationError error) => (ValidationErrors)error;
    public static implicit operator Result(ValidationErrors errors) => new(errors.AsEnumerable());
    public static implicit operator Result(ValidationError[] errors) => (ValidationErrors)errors;
    public static implicit operator Result(List<ValidationError> errors) => new ValidationErrors(errors);
    public static implicit operator Result(HashSet<ValidationError> errors) => (ValidationErrors)errors;
    public static implicit operator ValidationErrors(Result result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator ValidationError[](Result result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator Exception?(Result result) => result.Exception;

    [return: NotNullIfNotNull(nameof(left))]
    public static Result? operator +(Result? left, Result? right)
        => left is null ? right :
           right is null ? left :
           left.HasException ? left :
           right.HasException ? new(right.Exception) :
           new(left.Errors.Union(right.Errors));
    public static Result operator +(Result? left, ValidationErrors? right)
        => left is null ? right ?? [] :
           right is null ? left :
           left.HasException ? left :
           new(left.Errors.Union(right));
    public static Result operator +(Result? left, ValidationError? right)
        => left is null && right is null ? new() :
           left is null ? right! :
           right is null ? left :
           left.HasException ? left :
           new(left.Errors.Union([right]));

    public virtual bool Equals(Result? other)
        => other is not null
        && Errors.SequenceEqual(other.Errors);

    public override int GetHashCode()
        => Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine);

    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Invalid<TValue>(TValue value, string message, string? source = null) => Invalid(value, new ValidationError(message, source));
    public static Result<TValue> Invalid<TValue>(TValue value, Result result) => new(value, result.Errors);
    public static Result<TValue> Error<TValue>(string message) => Error<TValue>(new Exception(message));
    public static Result<TValue> Error<TValue>(Exception exception)
        => new(exception);

    public static Task<Result<TValue>> SuccessTask<TValue>(TValue value) => Task.FromResult(Success(value));
    public static Task<Result<TValue>> InvalidTask<TValue>(TValue value, string message, string? source = null) => InvalidTask(value, new ValidationError(message, source));
    public static Task<Result<TValue>> InvalidTask<TValue>(TValue value, Result result) => Task.FromResult(Invalid(value, result));
    public static Task<Result<TValue>> ErrorTask<TValue>(string message) => ErrorTask<TValue>(new Exception(message));
    public static Task<Result<TValue>> ErrorTask<TValue>(Exception exception)
        => Task.FromResult(Error<TValue>(exception));
}

public record Result<TValue> : Result, IResult<ResultType, TValue> {
    internal Result(Exception exception)
        : base(exception) {
    }

    public Result(TValue value, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Value = value;
    }

    public TValue Value { get; } = default!;

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Exception exception) => new(exception);
    public static implicit operator Result<TValue>(ValidationError error) => (ValidationErrors)error;
    public static implicit operator Result<TValue>(ValidationErrors errors) => new(default!, errors.AsEnumerable());
    public static implicit operator Result<TValue>(ValidationError[] errors) => (ValidationErrors)errors;
    public static implicit operator Result<TValue>(List<ValidationError> errors) => new ValidationErrors(errors);
    public static implicit operator Result<TValue>(HashSet<ValidationError> errors) => (ValidationErrors)errors;
    public static implicit operator ValidationErrors(Result<TValue> result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator ValidationError[](Result<TValue> result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator Exception?(Result<TValue> result) => result.Exception;
    public static implicit operator TValue(Result<TValue> result) => result.Value;

    [return: NotNullIfNotNull(nameof(left))]
    public static Result<TValue>? operator +(Result<TValue>? left, Result<TValue>? right)
        => right is null ? left :
           left is null ? right :
           left.HasException ? left :
           right.HasException ? new(right.Exception) :
           new(left.Value ?? right.Value, left.Errors.Union(right.Errors));
    public static Result<TValue> operator +(Result<TValue> left, ValidationError? right)
        => right is null ? left :
           left.HasException ? left :
           new(left.Value, left.Errors.Union([right]));
    public static Result<TValue> operator +(Result<TValue> left, ValidationErrors? right)
        => right is null ? left :
           left.HasException ? left :
           new(left.Value, left.Errors.Union(right));
    public static Result<TValue> operator +(Result<TValue> left, Result? right)
        => right is null ? left :
           left.HasException ? left :
           right.HasException ? new(right.Exception) :
           new(left.Value, left.Errors.Union(right.Errors));

    public Result<TNewValue> MapTo<TNewValue>(Func<TValue, TNewValue> map) {
        try {
            return HasException
                ? Error<TNewValue>(Exception)
                : new(map(Value), Errors);
        }
        catch (Exception ex) {
            return Error<TNewValue>(ex);
        }
    }
}
