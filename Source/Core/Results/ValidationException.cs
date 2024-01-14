namespace DotNetToolbox.Results;

[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "They are all implemented indirectly.")]
public class ValidationException(IEnumerable<ValidationError>? errors = null, Exception? innerException = null)
    : Exception(_defaultMessage, innerException) {
    private const string _defaultMessage = "Validation failed.";
    private const string _defaultError = "A validation error was found.";

    public ValidationError[] Errors { get; init; } = errors is IReadOnlyList<ValidationError> { Count: > 0 } array
                                                         ? errors.ToArray()
                                                         : [_defaultError];
    public ValidationException(string error, Exception? innerException = null)
        : this(string.Empty, error, innerException) {
    }

    public ValidationException(string source, string error, Exception? innerException = null)
        : this(new ValidationError(source, error), innerException) {
    }

    public ValidationException(ValidationError error, Exception? innerException = null)
        : this([error], innerException) {
    }
}
