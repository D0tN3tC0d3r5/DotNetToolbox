namespace DotNetToolbox.Results;

public record CrudResult : Result {
    private CrudResult(IResult result)
        : this(CrudResultType.Success, result.Errors, result.Exception) {
    }

    protected CrudResult(CrudResultType type, IEnumerable<ValidationError>? errors = null, Exception? exception = null)
        : base(errors, exception) {
        Type = HasException ? CrudResultType.Error : HasErrors ? CrudResultType.Invalid : type;
    }

    protected CrudResultType Type { get; init; }

    public override bool IsSuccess => Type is CrudResultType.Success;
    public override bool IsInvalid => Type is CrudResultType.Invalid;
    public bool WasNotFound => Type is CrudResultType.NotFound;
    public bool HasConflict => Type is CrudResultType.Conflict;

    public static new CrudResult Success() => new(CrudResultType.Success);
    public static CrudResult NotFound() => new(CrudResultType.NotFound);
    public static CrudResult Conflict() => new(CrudResultType.Conflict);
    public static new CrudResult Invalid([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(string.Empty, message, args);
    public static new CrudResult Invalid(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(new ValidationError(source, message, args));
    public static new CrudResult Invalid(Result result)
        => new(CrudResultType.Invalid, result.Errors);
    public static new CrudResult Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Error(new Exception(string.Format(message, args)));
    public static new CrudResult Error(Exception exception)
        => new(CrudResultType.Error, exception: exception);
    
    public static implicit operator CrudResult(ValidationError error)
        => new((Result)error);
    public static implicit operator CrudResult(List<ValidationError> errors)
        => new((Result)errors);
    public static implicit operator CrudResult(ValidationError[] errors)
        => new((Result)errors);
    public static implicit operator CrudResult(HashSet<ValidationError> errors)
        => new((Result)errors);
    public static implicit operator CrudResult(Exception exception)
        => new((Result)exception);

    public static CrudResult operator +(CrudResult left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(left.Type, errors, left.Exception ?? right.Exception);
    }

    public virtual bool Equals(CrudResult? other)
        => base.Equals(other)
        && Type == other.Type;

    public override int GetHashCode()
        => HashCode.Combine(base.GetHashCode(), Type);

    public static new CrudResult<TValue> Success<TValue>(TValue value)
        => new(CrudResultType.Success, value);
    public static CrudResult<TValue> NotFound<TValue>()
        => new(CrudResultType.NotFound);
    public static CrudResult<TValue> Conflict<TValue>(TValue value)
        => new(CrudResultType.Conflict, value);
    public static new CrudResult<TValue> Invalid<TValue>(TValue? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(value, string.Empty, message, args);
    public static new CrudResult<TValue> Invalid<TValue>(TValue? value, string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(value, new ValidationError(source, message, args));
    public static new CrudResult<TValue> Invalid<TValue>(TValue? value, Result result)
        => new(CrudResultType.Invalid, value, result.Errors);
    public static new CrudResult<TValue> Error<TValue>(TValue? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Error(value, new Exception(string.Format(message, args)));
    public static new CrudResult<TValue> Error<TValue>(TValue? value, Exception exception)
        => new(CrudResultType.Error, value, exception: exception);
}

public record CrudResult<TValue> : CrudResult {
    internal CrudResult(IResult result)
        : this(CrudResultType.Success, default, result.Errors, result.Exception) {
    }

    internal CrudResult(Result<TValue> result)
        : this(CrudResultType.Success, result.Value, result.Errors, result.Exception) {
    }

    internal CrudResult(CrudResultType type, TValue? value = default, IEnumerable<ValidationError>? errors = null, Exception? exception = null)
        : base(type, errors, exception) {
        Value = value;
    }

    public TValue? Value { get; init; }

    public static implicit operator CrudResult<TValue>(TValue? value) => new(value);
    public static implicit operator CrudResult<TValue>(Result<TValue> result) => new(result);

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
