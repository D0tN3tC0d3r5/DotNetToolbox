namespace DotNetToolbox.Results;

[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "They are all implemented indirectly.")]
public class ValidationException(string? message, string? source, ICollection<ValidationError> errors, Exception? innerException = null)
        : Exception(FormatSource(source) + FormatMessage(message), innerException) {
    public const string DefaultMessage = "Validation failed.";

    public ValidationError[] Errors { get; init; } = errors.Count > 0 ? errors.ToArray() : [ValidationError.DefaultErrorMessage];

    public ValidationException(Exception? innerException = null)
        : this(DefaultMessage, innerException) {
    }

    public ValidationException(string? message, Exception? innerException = null)
        : this(message ?? DefaultMessage, string.Empty, innerException) {
    }

    public ValidationException(string? message, string? source, Exception? innerException = null)
        : this(message ?? DefaultMessage, source ?? string.Empty, ValidationError.DefaultErrorMessage, innerException) {
    }

    public ValidationException(string? message, string? source, ValidationError error, Exception? innerException = null)
        : this(message ?? DefaultMessage, source ?? string.Empty, [error], innerException) {
    }

    public ValidationException(ValidationError error, Exception? innerException = null)
        : this(DefaultMessage, error, innerException) {
    }

    public ValidationException(string? message, ValidationError error, Exception? innerException = null)
        : this(message ?? DefaultMessage, string.Empty, error, innerException) {
    }

    public ValidationException(ICollection<ValidationError> errors, Exception? innerException = null)
        : this(DefaultMessage, errors, innerException) {
    }

    public ValidationException(string? message, ICollection<ValidationError> errors, Exception? innerException = null)
        : this(message ?? DefaultMessage, string.Empty, errors, innerException) {
    }

    private static string FormatSource(string? source) => string.IsNullOrEmpty(source) ? string.Empty : $"{source}: ";
    private static string FormatMessage(string? message) => string.IsNullOrWhiteSpace(message) ? DefaultMessage : message;
}
