namespace DotNetToolbox.Results;

public record Result : IResult {
    protected Result(IEnumerable<ValidationError>? errors = null, Exception? exception = null) {
        Errors = (HasNoNull(errors) ?? []).ToHashSet();
        Exception = exception;
    }

    public virtual bool IsSuccess => !IsInvalid;
    public ISet<ValidationError> Errors { get; init; } = new HashSet<ValidationError>();
    public Exception? Exception { get; }

    [MemberNotNullWhen(true, nameof(Exception))]
    public bool HasException => Exception is not null;
    public bool HasErrors => Errors.Count != 0;
    public virtual bool IsInvalid => HasException || HasErrors;

    private static readonly Result _success = new();
    public static Result Success() => _success;
    public static Result Invalid([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(string.Empty, message, args);
    public static Result Invalid(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(new ValidationError(source, message, args));
    public static Result Invalid(Result result)
        => new(result.Errors);
    public static Result Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Error(new Exception(string.Format(message, args)));
    public static Result Error(Exception exception)
        => new(exception: exception);

    public static implicit operator Result(ValidationError error)
        => new([ error ]);
    public static implicit operator Result(List<ValidationError> errors)
        => new([..errors]);
    public static implicit operator Result(HashSet<ValidationError> errors)
        => new([..errors]);
    public static implicit operator Result(ValidationError[] errors)
        => new(errors.AsEnumerable());
    public static implicit operator Result(Exception exception)
        => new(exception: exception);

    public static Result operator +(Result left, Result right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(errors, left.Exception ?? right.Exception);
    }

    public void EnsureIsSuccess() {
        if (HasException) throw Exception!;
        if (HasErrors) throw new ValidationException(Errors);
    }

    public virtual bool Equals(Result? other)
        => other is not null
        && Errors.SequenceEqual(other.Errors)
        && Equals(Exception, other.Exception);

    public override int GetHashCode()
        => HashCode.Combine(Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine), Exception);

    public static Result<TValue> Success<TValue>(TValue? value) => new(value);
    public static Result<TValue> Invalid<TValue>(TValue? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(value, string.Empty, message, args);
    public static Result<TValue> Invalid<TValue>(TValue? value, string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(value, new ValidationError(source, message, args));
    public static Result<TValue> Invalid<TValue>(TValue? value, Result result)
        => new(value, result.Errors);
    public static Result<TValue> Error<TValue>(TValue? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Error(value, new Exception(string.Format(message, args)));
    public static Result<TValue> Error<TValue>(TValue? value, Exception exception)
        => new(value, exception: exception);
}

public record Result<TValue> : Result {
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
