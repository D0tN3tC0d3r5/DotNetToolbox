namespace DotNetToolbox.Results;

public abstract record ResultBase<TType>
    : IResult<TType>
    where TType : Enum {
    private readonly ValidationErrors _errors;

    protected ResultBase(Exception exception)
        : this() {
        Exception = IsNotNull(exception);
    }

    protected ResultBase(IEnumerable<ValidationError>? errors = null) {
        _errors = new(AllAreNotNull(errors ?? []));
    }

    public abstract TType Type { get; }
    public Exception? Exception { get; }
    public IReadOnlyList<ValidationError> Errors => _errors;

    public bool HasErrors => Errors.Count != 0;
    [MemberNotNullWhen(true, nameof(Exception))]
    public bool HasException => Exception is not null;

    public void EnsureIsSuccess(string? message = null, string? source = null) {
        if (Exception is not null) throw new ValidationException(message ?? ValidationException.DefaultMessage, source ?? string.Empty, Exception);
        if (HasErrors) throw new ValidationException(message ?? ValidationException.DefaultMessage, source ?? string.Empty, [.. Errors]);
    }

    public virtual bool Equals(ResultBase<TType>? other)
        => other is not null
        && Equals(Type, other.Type)
        && Errors.SequenceEqual(other.Errors)
        && Equals(Exception, other.Exception);

    public override int GetHashCode()
        => HashCode.Combine(Type, Exception, Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine));
}
