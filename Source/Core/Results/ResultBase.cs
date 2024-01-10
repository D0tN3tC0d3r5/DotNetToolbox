using System.Collections.ObjectModel;

namespace DotNetToolbox.Results;

public record ResultBase : IResult {
    private readonly ObservableCollection<ValidationError> _errors = [];
    private readonly Exception? _exception;

    protected ResultBase(IEnumerable<ValidationError>? errors = null, Exception? exception = null) {
        Errors = (HasNoNull(errors) ?? []).ToList();
        Exception = exception;
    }

    protected virtual void OnErrorsChanged(IReadOnlyCollection<ValidationError> errors) { }
    protected virtual void OnExceptionChanged(Exception? exception) { }

    public ICollection<ValidationError> Errors {
        get => _errors;
        init {
            _errors = new(value);
            _errors.CollectionChanged += (_, _) => OnErrorsChanged(_errors);
            OnErrorsChanged(_errors);
        }
    }

    public Exception? Exception {
        get => _exception;
        private init {
            _exception = value;
            OnExceptionChanged(_exception);
        }
    }

    [MemberNotNullWhen(true, nameof(Exception))]
    public bool HasException => Exception is not null;
    public bool HasErrors => Errors.Count != 0;
    protected bool HasNoIssues => !HasErrors && !HasException;

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
