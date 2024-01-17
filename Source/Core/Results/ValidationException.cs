namespace DotNetToolbox.Results;

[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "They are all implemented indirectly.")]
public class ValidationException
    : Exception {
    public const string DefaultMessage = "Validation failed.";

    public ValidationError[] Errors { get; }

    public ValidationException(Exception? innerException = null)
        : this(DefaultMessage, innerException) {
    }

    public ValidationException(string message, Exception? innerException = null)
        : this(message, string.Empty, innerException) {
    }

    public ValidationException(string message, string? source, Exception? innerException = null)
        : this(message, source ?? string.Empty, ValidationError.DefaultErrorMessage, innerException) {
    }

    public ValidationException(string message, string? source, ValidationError error, Exception? innerException = null)
        : this(message, source ?? string.Empty, [error], innerException) {
    }

    public ValidationException(ValidationError error, Exception? innerException = null)
        : this(DefaultMessage, error, innerException) {
    }

    public ValidationException(string message, ValidationError error, Exception? innerException = null)
        : this(message, string.Empty, error, innerException) {
    }

    public ValidationException(ICollection<ValidationError> errors, Exception? innerException = null)
        : this(DefaultMessage, errors, innerException) {
    }

    public ValidationException(string message, ICollection<ValidationError> errors, Exception? innerException = null)
        : this(message, string.Empty, errors, innerException) {
    }

    public ValidationException(string? message, string? source, IEnumerable<ValidationError> errors, Exception? innerException = null)
        : base(FormatSource(source) + FormatMessage(message), innerException) {
        Errors = errors.Distinct().ToArray();
    }

    private static string FormatSource(string? source) => string.IsNullOrEmpty(source) ? string.Empty : $"{source}: ";
    private static string FormatMessage(string? message) => string.IsNullOrWhiteSpace(message) ? DefaultMessage : message;
}
