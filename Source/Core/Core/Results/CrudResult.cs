namespace DotNetToolbox.Results;

public record CrudResult : ResultBase<CrudResultType> {
    private readonly CrudResultType _type = CrudResultType.Success;

    public CrudResult() {
    }

    protected CrudResult(Exception exception)
        : base(exception) {
    }

    protected CrudResult(CrudResultType type, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        _type = type;
    }

    public override CrudResultType Type => HasException
        ? CrudResultType.Error
        : HasErrors
            ? CrudResultType.Invalid
            : _type;

    public bool IsSuccess => Type is CrudResultType.Success;
    public bool IsInvalid => Type is CrudResultType.Invalid;
    public bool IsFaulty => Type is CrudResultType.Error;
    public bool WasNotFound => Type is CrudResultType.NotFound;
    public bool HasConflict => Type is CrudResultType.Conflict;

    public static CrudResult Success() => new(CrudResultType.Success);
    public static CrudResult NotFound() => new(CrudResultType.NotFound);
    public static CrudResult Conflict() => new(CrudResultType.Conflict);
    public static CrudResult Invalid(Result result) => new(CrudResultType.Invalid, result.Errors);
    public static CrudResult Error(string error) => Error(new Exception(error));
    public static CrudResult Error(Exception exception) => new(exception);

    public static Task<CrudResult> SuccessTask() => Task.FromResult(Success());
    public static Task<CrudResult> NotFoundTask() => Task.FromResult(NotFound());
    public static Task<CrudResult> ConflictTask() => Task.FromResult(Conflict());
    public static Task<CrudResult> InvalidTask(Result result) => Task.FromResult(Invalid(result));
    public static Task<CrudResult> ErrorTask(string error) => ErrorTask(new Exception(error));
    public static Task<CrudResult> ErrorTask(Exception exception) => Task.FromResult(Error(exception));

    public static implicit operator CrudResult(Exception exception) => new(exception);
    public static implicit operator CrudResult(string error) => (Result)error;
    public static implicit operator CrudResult(ValidationError error) => (Result)error;
    public static implicit operator CrudResult(ValidationErrors errors) => (Result)errors;
    public static implicit operator CrudResult(ValidationError[] errors) => (Result)errors;
    public static implicit operator CrudResult(List<ValidationError> errors) => (Result)errors;
    public static implicit operator CrudResult(HashSet<ValidationError> errors) => (Result)errors;
    public static implicit operator CrudResult(Result result) => new(CrudResultType.Success, result.Errors);
    public static implicit operator ValidationErrors(CrudResult result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator ValidationError[](CrudResult result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator Exception?(CrudResult result) => result.Exception;

    public static CrudResult operator +(CrudResult left, Result right)
        => left.HasException
               ? left
               : right.HasException
                   ? new(right.Exception)
                   : new(left._type, left.Errors.Union(right.Errors));

    public static CrudResult<TValue> Success<TValue>(TValue value) => new(CrudResultType.Success, value);
    public static CrudResult<TValue> NotFound<TValue>() => new(CrudResultType.NotFound);
    public static CrudResult<TValue> Conflict<TValue>(TValue value) => new(CrudResultType.Conflict, value);
    public static CrudResult<TValue> Invalid<TValue>(TValue? value, Result result) => new(CrudResultType.Invalid, value, result.Errors);
    public static CrudResult<TValue> Error<TValue>(string error) => Error<TValue>(new Exception(error));
    public static CrudResult<TValue> Error<TValue>(Exception exception) => new(exception);

    public static Task<CrudResult<TValue>> SuccessTask<TValue>(TValue value) => Task.FromResult(Success(value));
    public static Task<CrudResult<TValue>> NotFoundTask<TValue>() => Task.FromResult(NotFound<TValue>());
    public static Task<CrudResult<TValue>> ConflictTask<TValue>(TValue value) => Task.FromResult(Conflict(value));
    public static Task<CrudResult<TValue>> InvalidTask<TValue>(TValue? value, Result result) => Task.FromResult(Invalid(value, result));
    public static Task<CrudResult<TValue>> ErrorTask<TValue>(string error) => ErrorTask<TValue>(new Exception(error));
    public static Task<CrudResult<TValue>> ErrorTask<TValue>(Exception exception) => Task.FromResult(Error<TValue>(exception));
}

public record CrudResult<TValue> : CrudResult, IResult<CrudResultType, TValue> {
    private readonly CrudResultType _type = CrudResultType.Success;

    internal CrudResult(Exception exception)
        : base(exception) {
    }

    internal CrudResult(CrudResultType type, TValue? value = default, IEnumerable<ValidationError>? errors = null)
        : base(type, errors) {
        _type = type;
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator CrudResult<TValue>(TValue? value) => new(CrudResultType.Success, value);
    public static implicit operator CrudResult<TValue>(Result<TValue> result) => new(CrudResultType.Success, result.Value, result.Errors);
    public static implicit operator CrudResult<TValue>(Exception exception) => new(exception);
    public static implicit operator CrudResult<TValue>(ValidationError error) => (ValidationErrors)error;
    public static implicit operator CrudResult<TValue>(ValidationErrors errors) => new(CrudResultType.Success, default!, errors.AsEnumerable());
    public static implicit operator CrudResult<TValue>(ValidationError[] errors) => (ValidationErrors)errors;
    public static implicit operator CrudResult<TValue>(List<ValidationError> errors) => new ValidationErrors(errors);
    public static implicit operator CrudResult<TValue>(HashSet<ValidationError> errors) => (ValidationErrors)errors;
    public static implicit operator ValidationErrors(CrudResult<TValue> result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator ValidationError[](CrudResult<TValue> result) => result.HasException ? [] : [.. result.Errors];
    public static implicit operator Exception?(CrudResult<TValue> result) => result.Exception;
    public static implicit operator TValue?(CrudResult<TValue> result) => result.Value;

    public static CrudResult<TValue> operator +(CrudResult<TValue> left, Result right)
        => left.HasException
               ? left
               : right.HasException
                   ? new(right.Exception)
                   : new(left._type, left.Value, left.Errors.Union(right.Errors));

    public CrudResult<TNewValue> MapTo<TNewValue>(Func<TValue?, TNewValue?> map) {
        try {
            return HasException
                ? Error<TNewValue>(Exception)
                : Type is CrudResultType.NotFound
                    ? NotFound<TNewValue>()
                    : new(Type, map(Value), Errors);
        }
        catch (Exception ex) {
            return Error<TNewValue>(ex);
        }
    }
}
