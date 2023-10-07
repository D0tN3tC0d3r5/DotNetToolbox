namespace System.Results;

public abstract record Result<TResult, TType> : IResult<TType>
    where TResult : Result<TResult, TType>
    where TType : Enum {
    protected Result(TType defaultType, TType type, IEnumerable<ValidationError>? errors = null) {
        ValidationErrors = (errors is null
            ? Enumerable.Empty<ValidationError>()
            : IsNotNullAndDoesNotContainNull(errors).Distinct()).ToImmutableArray();
        Type = ValidationErrors.Count != 0 ? defaultType : type;
    }

    public TType Type { get; }
    public IReadOnlyList<ValidationError> ValidationErrors { get; init; }
    protected bool HasErrors => ValidationErrors.Count > 0;

    public void EnsureIsValid(string? message = null) { 
        if (HasErrors) throw new ValidationException(message, ValidationErrors);
    }

    public virtual bool Equals(TResult? other)
        => other is not null
           && Type.Equals(other.Type)
           && ValidationErrors.SequenceEqual(other.ValidationErrors);

    public override int GetHashCode()
        => ValidationErrors.Aggregate(Type.GetHashCode(), (s, ve) => HashCode.Combine(s, ve.GetHashCode()));
}
