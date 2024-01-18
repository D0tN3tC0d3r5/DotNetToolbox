namespace DotNetToolbox.Results;

[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "They are all implemented indirectly.")]
public class ValidationException(string message, string source, IEnumerable<ValidationError> errors, Exception? innerException = null)
    : Exception(FormatSource(source) + FormatMessage(message), innerException) {
    public const string DefaultMessage = "Validation failed.";

    public ValidationError[] Errors { get; } = errors.Distinct().ToArray();

    public ValidationException(Exception? innerException = null)
        : this(DefaultMessage, innerException) {
    }

    public ValidationException(ValidationError error, Exception? innerException = null)
        : this(DefaultMessage, error, innerException) {
    }

    public ValidationException(IEnumerable<ValidationError> errors, Exception? innerException = null)
        : this(DefaultMessage, errors, innerException) {
    }

    public ValidationException(string message, Exception? innerException = null)
        : this(message, string.Empty, innerException) {
    }

    public ValidationException(string message, ValidationError error, Exception? innerException = null)
        : this(message, string.Empty, error, innerException) {
    }

    public ValidationException(string message, IEnumerable<ValidationError> errors, Exception? innerException = null)
        : this(message, string.Empty, errors, innerException) {
    }

    public ValidationException(string message, string source, Exception? innerException = null)
        : this(message, source, ValidationError.DefaultErrorMessage, innerException) {
    }

    public ValidationException(string message, string source, ValidationError error, Exception? innerException = null)
        : this(message, source, [error], innerException) {
    }

    private static string FormatMessage(string message) => IsNotNullOrWhiteSpace(message);
    private static string FormatSource(string source) => string.IsNullOrEmpty(source) ? string.Empty : $"{source}: ";
}
