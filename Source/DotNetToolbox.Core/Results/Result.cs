namespace System.Results;

public abstract record Result<TResult, TType> : IResult<TType>
    where TResult : Result<TResult, TType>
    where TType : Enum {
    protected Result(TType defaultType, TType type, IEnumerable<ValidationError>? errors = null) {
        Errors = (errors is null
            ? Enumerable.Empty<ValidationError>()
            : IsNotNullAndDoesNotContainNull(errors).Distinct()).ToImmutableArray();
        Type = Errors.Count != 0 ? defaultType : type;
    }

    public TType Type { get; }
    public IReadOnlyList<ValidationError> Errors { get; init; }
    public bool IsValid => Errors.Count == 0;
    public bool IsInvalid => Errors.Count != 0;

    public void EnsureIsValid(string? message = null) { 
        if (!IsValid) throw new ValidationException(message, Errors);
    }

    public virtual bool Equals(TResult? other)
        => other is not null
           && Type.Equals(other.Type)
           && Errors.SequenceEqual(other.Errors);

    public override int GetHashCode()
        => Errors.Aggregate(Type.GetHashCode(), (s, ve) => HashCode.Combine(s, ve.GetHashCode()));
}
