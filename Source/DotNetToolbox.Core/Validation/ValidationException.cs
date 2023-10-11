namespace System.Validation;

public class ValidationException
    : Exception {
    private const string _defaultMessage = "Validation failed.";

    public ValidationException(string? message = null, IEnumerable<ValidationError>? errors = null, Exception? innerException = null)
        : base(message ?? _defaultMessage, innerException) {
        Errors = errors?.ToArray() ?? new[] { new ValidationError(message ?? _defaultMessage) };
    }

    public IReadOnlyList<ValidationError> Errors { get; }
}
