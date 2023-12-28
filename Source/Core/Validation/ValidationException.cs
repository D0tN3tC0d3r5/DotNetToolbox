namespace DotNetToolbox.Validation;

public class ValidationException(IEnumerable<ValidationError> errors) : Exception(_defaultMessage) {
    private const string _defaultMessage = "Validation failed.";

    public ValidationException(string error)
        : this(string.Empty, error) {
    }

    public ValidationException(string source, string error)
        : this(new ValidationError(source, error)) {
    }

    public ValidationException(ValidationError error)
        : this(new[] { error }) {
    }

    public IEnumerable<ValidationError> Errors { get; } = errors.ToArray();
}
