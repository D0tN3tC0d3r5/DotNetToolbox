namespace DotNetToolbox.Results;

public record Result : IResult {
    protected Result(IEnumerable<ValidationError>? errors = null) {
        Errors = errors is null
            ? []
            : DoesNotHaveNulls(errors).ToHashSet();
    }

    public ISet<ValidationError> Errors { get; init; } = new HashSet<ValidationError>();
    protected bool HasErrors => Errors.Count != 0;
    public virtual bool IsInvalid => HasErrors;
    public virtual bool IsSuccess => !HasErrors;

    public virtual bool Equals(Result? other)
        => other is not null
           && Errors.SequenceEqual(other.Errors);

    public override int GetHashCode()
        => Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine);

    private static readonly Result _success = new();
    public static Result Success() => _success;
    public static Result Invalid([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(string.Empty, message, args);
    public static Result Invalid(string source, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
        => Invalid(new ValidationError(source, message, args));
    public static Result Invalid(Result result)
        => new(result.Errors);

    public static implicit operator Result(List<ValidationError> errors)
        => new(errors.AsEnumerable());
    public static implicit operator Result(ValidationError[] errors)
        => new(errors.AsEnumerable());
    public static implicit operator Result(HashSet<ValidationError> errors)
        => new(errors.AsEnumerable());
    public static implicit operator Result(ValidationError error)
        => new(new[] { error, }.AsEnumerable());

    public static Result operator +(Result left, Result right)
        => left with { Errors = left.Errors.Union(right.Errors).ToHashSet(), };

    public void EnsureIsValid() {
        if (HasErrors) throw new ValidationException(Errors);
    }

    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Invalid<TValue>(TValue value, string message, string source, params object[] args) => new(value, new ValidationError[] { new(source, message, args), });
}

public record Result<TResult> : Result {
    internal Result(TResult value, IEnumerable<ValidationError>? errors = null)
        : base(errors) {
        Value = IsNotNull(value);
    }

    public TResult Value { get; init; }

    public static implicit operator Result<TResult>(TResult value) => new(value);

    public static Result<TResult> operator +(Result<TResult> left, Result right)
        => left with { Errors = left.Errors.Union(right.Errors).ToHashSet(), };

    public Result<TOutput> MapTo<TOutput>(Func<TResult, TOutput> map)
        => new(map(Value), Errors);
}
