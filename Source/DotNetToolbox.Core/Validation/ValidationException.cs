namespace System.Validation;

public class ValidationException
    : Exception {
    private const string _defaultMessage = "Validation failed.";

    public ValidationException(IEnumerable<ValidationError> errors, string? message = null)
        : base(message ?? _defaultMessage) {
        Errors = errors.ToArray();
    }

    public IReadOnlyList<ValidationError> Errors { get; }
}
