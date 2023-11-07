namespace System.Results;

public class  ResultBase : IResult {
    protected ResultBase(IEnumerable<ValidationError>? errors = null) {
        Errors = errors is null
            ? []
            : DoesNotHaveNulls(errors).ToHashSet();
    }

    public ISet<ValidationError> Errors { get; }
    protected bool HasErrors => Errors.Count != 0;

    public static implicit operator ResultBase(ValidationError[] errors)
        => new(errors.AsEnumerable());
    public static implicit operator ResultBase(ValidationError error)
        => new(new[] { error, }.AsEnumerable());

    public static bool operator ==(ResultBase left, ResultBase? right)
        => left.Equals(right);
    public static bool operator !=(ResultBase left, ResultBase? right)
        => !left.Equals(right);
    public static ResultBase operator +(ResultBase left, ResultBase right)
        => new(left.Errors.Union(right.Errors));

    public override bool Equals([NotNullWhen(true)] object? obj)
        => ReferenceEquals(obj, this)
        || obj is ResultBase r && Equals(r);
    public override int GetHashCode()
        => Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine);

    private bool Equals(IResult? other)
        => other is not null
        && Errors.SequenceEqual(other.Errors);

    public void EnsureIsValid() {
        if (HasErrors) throw new ValidationException(Errors);
    }
}
