namespace System.Results;

public class  ResultBase : IResult {
    protected ResultBase(IEnumerable<ValidationError>? errors = null) {
        Errors = errors is null
            ? []
            : DoesNotHaveNulls(errors).ToHashSet();
    }

    public ISet<ValidationError> Errors { get; init; } = new HashSet<ValidationError>();
    protected bool HasErrors => Errors.Count != 0;

    public virtual bool Equals([NotNullWhen(true)] ResultBase? other)
        => other is not null
        && Errors.SequenceEqual(other.Errors);

    public override int GetHashCode()
        => Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine);

    public static implicit operator ResultBase(ValidationError[] errors)
        => new(errors.AsEnumerable());
    public static implicit operator ResultBase(ValidationError error)
        => new(new[] { error, }.AsEnumerable());

    public static ResultBase operator +(ResultBase left, ResultBase right) {
        left.Errors.UnionWith(right.Errors);
        return left;
    }

    public void EnsureIsValid() {
        if (Errors.Count != 0) throw new ValidationException(Errors);
    }
}
