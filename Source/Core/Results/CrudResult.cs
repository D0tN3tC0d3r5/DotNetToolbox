namespace DotNetToolbox.Results;

public record CrudResult : ResultBase {
    private CrudResult(IResult result)
        : this(CrudResultType.Success, result.Errors) {
    }

    protected CrudResult(Exception exception)
        : base(exception) {
        Type = CrudResultType.Error;
    }

    protected CrudResult(CrudResultType type, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
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

    public bool IsSuccess => Type is CrudResultType.Success;
    public bool IsInvalid => Type is CrudResultType.Invalid;
    public bool IsFaulty => Type is CrudResultType.Error;
    public bool WasNotFound => Type is CrudResultType.NotFound;
    public bool HasConflict => Type is CrudResultType.Conflict;

    public static CrudResult Success() => new(CrudResultType.Success);
    public static CrudResult NotFound() => new(CrudResultType.NotFound);
    public static CrudResult Conflict() => new(CrudResultType.Conflict);
    public static CrudResult Invalid(Result result) => new(CrudResultType.Invalid, result.Errors);
    public static CrudResult Exception(Exception exception) => new(exception);

    public static Task<CrudResult> SuccessTask() => Task.FromResult(Success());
    public static Task<CrudResult> NotFoundTask() => Task.FromResult(NotFound());
    public static Task<CrudResult> ConflictTask() => Task.FromResult(Conflict());
    public static Task<CrudResult> InvalidTask(Result result) => Task.FromResult(Invalid(result));
    public static Task<CrudResult> ExceptionTask(Exception exception) => Task.FromResult(Exception(exception));

    public static implicit operator CrudResult(ValidationError error) => new((Result)error);
    public static implicit operator CrudResult(List<ValidationError> errors) => new((Result)errors);
    public static implicit operator CrudResult(ValidationError[] errors) => new((Result)errors);
    public static implicit operator CrudResult(HashSet<ValidationError> errors) => new((Result)errors);
    public static implicit operator CrudResult(Exception exception) => new(exception);
    public static implicit operator CrudResult(Result result) => new((IResult)result);

    public static CrudResult operator +(CrudResult left, CrudResult right)
        => new(right.Type, left.Errors.Union(right.Errors).ToHashSet());

    public static CrudResult operator +(CrudResult left, Result right)
        => new(left.Type, left.Errors.Union(right.Errors).ToHashSet());

    public virtual bool Equals(CrudResult? other)
        => base.Equals(other)
        && Type == other.Type;

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Type);

    public static CrudResult<TValue> Success<TValue>(TValue value) => new(CrudResultType.Success, value);
    public static CrudResult<TValue> NotFound<TValue>() => new(CrudResultType.NotFound);
    public static CrudResult<TValue> Conflict<TValue>(TValue value) => new(CrudResultType.Conflict, value);
    public static CrudResult<TValue> Invalid<TValue>(TValue? value, Result result) => new(CrudResultType.Invalid, value, result.Errors);
    public static CrudResult<TValue> Exception<TValue>(Exception exception) => new(exception);

    public static Task<CrudResult<TValue>> SuccessTask<TValue>(TValue value) => Task.FromResult(Success(value));
    public static Task<CrudResult<TValue>> NotFoundTask<TValue>() => Task.FromResult(NotFound<TValue>());
    public static Task<CrudResult<TValue>> ConflictTask<TValue>(TValue value) => Task.FromResult(Conflict(value));
    public static Task<CrudResult<TValue>> InvalidTask<TValue>(TValue? value, Result result) => Task.FromResult(Invalid(value, result));
    public static Task<CrudResult<TValue>> ExceptionTask<TValue>(Exception exception) => Task.FromResult(Exception<TValue>(exception));
}

public record CrudResult<TValue> : CrudResult, IResult<TValue> {
    internal CrudResult(IResult<TValue> result)
        : this(CrudResultType.Success, result.Value, result.Errors) {
    }

    internal CrudResult(Exception exception)
        : base(exception) {
    }

    internal CrudResult(CrudResultType type, TValue? value = default, IEnumerable<ValidationError>? errors = null)
        : base(type, errors) {
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator CrudResult<TValue>(TValue? value) => new(CrudResultType.Success, value);
    public static implicit operator CrudResult<TValue>(Result<TValue> result) => new((IResult<TValue>)result);

    public static CrudResult<TValue> operator +(CrudResult<TValue> left, CrudResult right)
        => new(right.Type, left.Value, left.Errors.Union(right.Errors));

    public static CrudResult<TValue> operator +(CrudResult<TValue> left, Result right)
        => new(left.Type, left.Value, left.Errors.Union(right.Errors));

    public CrudResult<TNewValue> MapTo<TNewValue>(Func<TValue?, TNewValue?> map) {
        try {
            return HasException
                ? Exception<TNewValue>(InnerException)
                : Type is CrudResultType.NotFound
                    ? NotFound<TNewValue>()
                    : new(Type, map(Value), Errors);
        }
        catch (Exception ex) {
            return Exception<TNewValue>(ex);
        }
    }

    public virtual bool Equals(CrudResult<TValue>? other)
        => base.Equals(other)
        && Equals(Value, other.Value);

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Value);
}
