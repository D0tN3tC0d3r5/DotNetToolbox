using static System.Ensure;

namespace System.Results;

public record CrudResult : Result {
    protected CrudResult(CrudResultType type, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Type = HasErrors ? CrudResultType.Invalid : type;
    }

    protected CrudResultType Type { get; set; }

    public override bool IsSuccess => base.IsSuccess && Type is CrudResultType.Success;
    public override bool IsInvalid => base.IsInvalid || Type is CrudResultType.Invalid;
    public bool WasNotFound => !IsInvalid && Type is CrudResultType.NotFound;
    public bool HasConflict => !IsInvalid && Type is CrudResultType.Conflict;

    public static new CrudResult Success() => new(CrudResultType.Success);
    public static CrudResult NotFound() => new(CrudResultType.NotFound);
    public static CrudResult Conflict() => new(CrudResultType.Conflict);

    public static new CrudResult Invalid(string message, string source, params object?[] args)
        => new(new ValidationError(message, source, args));
    public static CrudResult Invalid(Result result)
        => new(CrudResultType.Invalid, result.Errors);

    public static implicit operator CrudResult(List<ValidationError> errors)
        => new(CrudResultType.Invalid, IsNotNullAndDoesNotHaveNull(errors));
    public static implicit operator CrudResult(ValidationError[] errors)
        => new(CrudResultType.Invalid, IsNotNullAndDoesNotHaveNull(errors));
    public static implicit operator CrudResult(ValidationError error)
        => new(CrudResultType.Invalid, new[] { error }.AsEnumerable());

    public static CrudResult operator +(CrudResult left, Result right) {
        left.Errors.Merge(right.Errors.Distinct());
        left.Type = left.IsInvalid ? CrudResultType.Invalid : left.Type;
        return left;
    }

    public static new CrudResult<TValue> Success<TValue>(TValue value)
        => new(CrudResultType.Success, IsNotNull(value));
    public static CrudResult<TValue> NotFound<TValue>()
        => new(CrudResultType.NotFound);
    public static CrudResult<TValue> Conflict<TValue>(TValue value)
        => new(CrudResultType.Conflict, IsNotNull(value));
    public static CrudResult<TValue> Invalid<TValue>(TValue value, string message, string source)
        => new(CrudResultType.Invalid, IsNotNull(value), new ValidationError[] { new(message, source) });
    public static CrudResult<TValue> Invalid<TValue>(TValue value, IEnumerable<ValidationError> errors)
        => new(CrudResultType.Invalid, IsNotNull(value), errors);
}

public record CrudResult<TResult> : CrudResult {
    internal CrudResult(CrudResultType type, TResult? value = default, IEnumerable<ValidationError>? errors = null)
        : base(type, errors) {
        Value = value;
    }

    public TResult? Value { get; }

    public static implicit operator CrudResult<TResult>(TResult? value) => new(CrudResultType.Success, value);
    public static implicit operator CrudResult<TResult>(Result<TResult> result)
        => new(result.IsInvalid ? CrudResultType.Invalid : CrudResultType.Success, result.Value, result.Errors);

    public static CrudResult<TResult> operator +(CrudResult<TResult> left, Result right) {
        left.Errors.Merge(right.Errors.Distinct());
        left.Type = left.IsInvalid ? CrudResultType.Invalid : left.Type;
        return left;
    }

    public CrudResult<TOutput> MapTo<TOutput>(Func<TResult, TOutput> map)
        => Value is null
            ? NotFound<TOutput>()
            : new(Type, map(Value), Errors);
}
