namespace DotNetToolbox.Results;

public abstract record ResultBase
    : IResultBase {
    protected ResultBase(IEnumerable<Error>? errors = null) {
        Errors = errors as List<Error> ?? errors?.ToList() ?? [];
    }

    /// <summary>
    /// The collection of unique errors. If empty, the Result is considered a Success.
    /// </summary>
    public IReadOnlyList<Error> Errors { get; }

    /// <summary>
    /// True if the result has at least one error.
    /// </summary>
    public bool HasErrors => Errors.Count != 0;

    /// <summary>
    /// A Success result has no errors.
    /// </summary>
    public bool IsSuccessful => !HasErrors;

    /// <summary>
    /// A Failure result has at least one error.
    /// </summary>
    public bool IsFailure => HasErrors;

    public void EnsureIsSuccess() {
        if (IsFailure) throw new OperationFailureException(Errors);
    }

    public virtual bool Equals(ResultBase? other)
        => other?.Errors.SequenceEqual(Errors) ?? false;

    public override int GetHashCode()
        => Errors.Aggregate(0, static (s, i) => HashCode.Combine(s, i.GetHashCode()));
}
