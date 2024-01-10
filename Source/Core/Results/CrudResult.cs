namespace DotNetToolbox.Results;

public record CrudResult : ResultBase {
    private CrudResult(IResult result)
        : this(CrudResultType.Success, result.Errors, result.Exception) {
    }

    protected CrudResult(CrudResultType type, IEnumerable<ValidationError>? errors = null, Exception? exception = null)
        : base(errors, exception) {
        SetType(type);
    }

    internal CrudResultType Type { get; private set; }

    private void SetType(CrudResultType type)
        => Type = HasException
                      ? CrudResultType.Error
                      : HasErrors
                          ? CrudResultType.Invalid
                          : type;

    protected override void OnErrorsChanged(IReadOnlyCollection<ValidationError> errors)
        => SetType(Type);

    protected override void OnExceptionChanged(Exception? exception)
        => SetType(Type);

    public bool IsSuccess => Type is CrudResultType.Success;
    public bool IsInvalid => Type is CrudResultType.Invalid;
    public bool WasNotFound => Type is CrudResultType.NotFound;
    public bool HasConflict => Type is CrudResultType.Conflict;

    public static CrudResult Success() => new(CrudResultType.Success);
    public static CrudResult NotFound() => new(CrudResultType.NotFound);
    public static CrudResult Conflict() => new(CrudResultType.Conflict);
    public static CrudResult InvalidData(Result result) => new(CrudResultType.Invalid, result.Errors);
    public static CrudResult Error(Exception exception) => new(CrudResultType.Error, exception: exception);

    public static Task<CrudResult> SuccessTask() => Task.FromResult(Success());
    public static Task<CrudResult> NotFoundTask() => Task.FromResult(NotFound());
    public static Task<CrudResult> ConflictTask() => Task.FromResult(Conflict());
    public static Task<CrudResult> InvalidDataTask(Result result) => Task.FromResult(InvalidData(result));
    public static Task<CrudResult> ErrorTask(Exception exception) => Task.FromResult(Error(exception));

    public static implicit operator CrudResult(ValidationError error) => new((Result)error);
    public static implicit operator CrudResult(List<ValidationError> errors) => new((Result)errors);
    public static implicit operator CrudResult(ValidationError[] errors) => new((Result)errors);
    public static implicit operator CrudResult(HashSet<ValidationError> errors) => new((Result)errors);
    public static implicit operator CrudResult(Exception exception) => new((Result)exception);
    public static implicit operator CrudResult(Result result) => new((IResult)result);

    public static CrudResult operator +(CrudResult left, CrudResult right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(right.Type, errors, left.Exception ?? right.Exception);
    }
    public static CrudResult operator +(CrudResult left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(left.Type, errors, left.Exception ?? right.Exception);
    }

    public virtual bool Equals(CrudResult? other)
        => base.Equals(other)
        && Type == other.Type;

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Type);

    public static CrudResult<TValue> Success<TValue>(TValue value) => new(CrudResultType.Success, value);
    public static CrudResult<TValue> NotFound<TValue>() => new(CrudResultType.NotFound);
    public static CrudResult<TValue> Conflict<TValue>(TValue value) => new(CrudResultType.Conflict, value);
    public static CrudResult<TValue> InvalidData<TValue>(TValue? value, Result result) => new(CrudResultType.Invalid, value, result.Errors);
    public static CrudResult<TValue> Error<TValue>(TValue? value, Exception exception) => new(CrudResultType.Error, value, exception: exception);

    public static Task<CrudResult<TValue>> SuccessTask<TValue>(TValue value) => Task.FromResult(Success(value));
    public static Task<CrudResult<TValue>> NotFoundTask<TValue>() => Task.FromResult(NotFound<TValue>());
    public static Task<CrudResult<TValue>> ConflictTask<TValue>(TValue value) => Task.FromResult(Conflict(value));
    public static Task<CrudResult<TValue>> InvalidDataTask<TValue>(TValue? value, Result result) => Task.FromResult(InvalidData(value, result));
    public static Task<CrudResult<TValue>> ErrorTask<TValue>(TValue? value, Exception exception) => Task.FromResult(Error(value, exception));
}

public record CrudResult<TValue> : CrudResult, IResult<TValue> {
    internal CrudResult(IResult<TValue> result)
        : this(CrudResultType.Success, result.Value, result.Errors, result.Exception) {
    }

    internal CrudResult(CrudResultType type, TValue? value = default, IEnumerable<ValidationError>? errors = null, Exception? exception = null)
        : base(type, errors, exception) {
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator CrudResult<TValue>(TValue? value) => new(CrudResultType.Success, value);
    public static implicit operator CrudResult<TValue>(Result<TValue> result) => new((IResult<TValue>)result);

    public static CrudResult<TValue> operator +(CrudResult<TValue> left, CrudResult right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(right.Type, left.Value, errors, left.Exception ?? right.Exception);
    }
    public static CrudResult<TValue> operator +(CrudResult<TValue> left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(left.Type, left.Value, errors, left.Exception ?? right.Exception);
    }

    public CrudResult<TNewValue> MapTo<TNewValue>(Func<TValue?, TNewValue?> map)
        => Type is CrudResultType.NotFound
            ? NotFound<TNewValue>()
            : new(Type, map(Value), Errors, Exception);

    public virtual bool Equals(CrudResult<TValue>? other)
        => base.Equals(other)
        && Equals(Value, other.Value);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value);
}
