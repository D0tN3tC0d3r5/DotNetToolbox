namespace System.Validation;

public class ValidationException
    : Exception {
    private const string _defaultMessage = "Validation failed.";

    public ValidationException(string error)
        : this(string.Empty, error) {
    }

    public ValidationException(string source, string error)
        : this(new ValidationError(source, error)) {
    }

    public ValidationException(ValidationError error)
        : this(new[] { error, }) {
    }

    public ValidationException(IEnumerable<ValidationError> errors)
        : base(_defaultMessage) {
        Errors = errors.ToArray();
    }

    public IEnumerable<ValidationError> Errors { get; }
}
