namespace DotNetToolbox.Results;

public record ResultBase : IResult {
    private readonly ObservableCollection<ValidationError> _errors = [];

    protected ResultBase(IEnumerable<ValidationError>? errors = null) {
        errors = (AllAreNotNull(errors) ?? []).ToArray();
        var exception = errors.Where(i => i.Exception is not null).Take(1).ToArray();
        Errors = (exception.Length == 0 ? exception : errors).ToHashSet();
    }

    protected virtual void OnErrorsChanged(IReadOnlyCollection<ValidationError> errors) { }
    public ICollection<ValidationError> Errors {
        get => _errors;
        init {
            _errors = new(value);
            _errors.CollectionChanged += (_, _) => OnErrorsChanged(_errors);
            OnErrorsChanged(_errors);
        }
    }

    public bool HasErrors => Errors.Count != 0;
    public bool HasException => Errors.Any(i => i.Exception is not null);
    protected bool HasNoIssues => !HasErrors;

    public static implicit operator ResultBase(string error)
        => new((ValidationError)error);
    public static implicit operator ResultBase(Exception exception)
        => new((ValidationError)exception);
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

    public void EnsureIsSuccess() {
        var exception = Errors.FirstOrDefault(i => i.Exception is not null).Exception;
        if (exception is not null) throw exception;
        if (HasErrors) throw new ValidationException(Errors);
    }

    public virtual bool Equals(ResultBase? other)
        => other is not null
        && Errors.SequenceEqual(other.Errors);

    public override int GetHashCode()
        => HashCode.Combine(Errors.Aggregate(Array.Empty<ValidationError>().GetHashCode(), HashCode.Combine));
}
