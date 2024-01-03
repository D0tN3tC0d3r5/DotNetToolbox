namespace DotNetToolbox.Results;

public record ResultBase : IResult {
    protected ResultBase(IEnumerable<ValidationError>? errors = null, Exception? exception = null) {
        Errors = (HasNoNull(errors) ?? []).ToHashSet();
        Exception = exception;
    }

    public ISet<ValidationError> Errors { get; init; } = new HashSet<ValidationError>();
    public Exception? Exception { get; }

    [MemberNotNullWhen(true, nameof(Exception))]
    public bool HasException => Exception is not null;
    public bool HasErrors => Errors.Count != 0;

    public static implicit operator ResultBase(ValidationError error)
        => new([error]);
    public static implicit operator ResultBase(List<ValidationError> errors)
        => new([.. errors]);
    public static implicit operator ResultBase(HashSet<ValidationError> errors)
        => new([.. errors]);
    public static implicit operator ResultBase(ValidationError[] errors)
        => new(errors.AsEnumerable());
    public static implicit operator ResultBase(Exception exception)
        => new(exception: exception);

    public static ResultBase operator +(ResultBase left, IResult right) {
        var errors = left.Errors.Union(right.Errors).ToHashSet();
        return new(errors, left.Exception ?? right.Exception);
    }

    public void EnsureIsSuccess() {
        if (HasException) throw Exception!;
        if (HasErrors) throw new ValidationException(Errors);
    }

    public virtual bool Equals(ResultBase? other)
        => other is not null
        && Errors.SequenceEqual(other.Errors)
        && Equals(Exception, other.Exception);

    public override int GetHashCode()
        => HashCode.Combine(Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine), Exception);
}
