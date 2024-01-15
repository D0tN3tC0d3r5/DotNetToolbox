namespace DotNetToolbox.Results;

public record ResultBase : IResult {
    private readonly ObservableCollection<ValidationError> _errors = [];

    protected ResultBase(Exception exception) {
        Exception = exception;
    }

    protected ResultBase(IEnumerable<ValidationError>? errors = null) {
        Errors = (AllAreNotNull(errors) ?? []).ToArray();
    }

    protected virtual void OnErrorsChanged(IReadOnlyCollection<ValidationError> errors) { }
    public IReadOnlyList<ValidationError> Errors {
        get => _errors.ToArray();
        init {
            _errors = [.. value.Distinct()];
            _errors.CollectionChanged += (_, _) => OnErrorsChanged(_errors);
            OnErrorsChanged(_errors);
        }
    }

    public bool HasErrors => Errors.Count != 0;
    public Exception? Exception { get; }
    [MemberNotNullWhen(true, nameof(Exception))]
    public bool HasException => Exception is not null;

    public static implicit operator ResultBase(string error)
        => new((ValidationError)error);
    public static implicit operator ResultBase(Exception exception)
        => new(exception);
    public static implicit operator ResultBase(ValidationError error)
        => new([error]);
    public static implicit operator ResultBase(List<ValidationError> errors)
        => new([.. errors]);
    public static implicit operator ResultBase(HashSet<ValidationError> errors)
        => new([.. errors]);
    public static implicit operator ResultBase(ValidationError[] errors)
        => new(errors.AsEnumerable());

    public static ResultBase operator +(ResultBase left, IResult right)
        => new(left.Errors.Union(right.Errors));

    public void EnsureIsSuccess(string? message = null, string? source = null) {
        if (Exception is not null) throw new ValidationException(message, source, Exception);
        if (HasErrors) throw new ValidationException(message, source, [.. Errors]);
    }

    public virtual bool Equals(ResultBase? other)
        => other is not null
        && Errors.SequenceEqual(other.Errors);

    public override int GetHashCode()
        => HashCode.Combine(Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine));
}
