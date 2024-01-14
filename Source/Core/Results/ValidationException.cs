namespace DotNetToolbox.Results;

[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "They are all implemented indirectly.")]
public class ValidationException(IEnumerable<ValidationError>? errors = null)
    : Exception(_defaultMessage, null) {
    private const string _defaultMessage = "Validation failed.";
    private const string _defaultError = "A validation error was found.";

    public ValidationError[] Errors { get; init; }
        = errors is IReadOnlyList<ValidationError> { Count: > 0 }
            ? errors.ToArray()
            : [_defaultError];
    public ValidationException(string error)
        : this(string.Empty, error) {
    }

    public ValidationException(string source, string error)
        : this(new ValidationError(source, error)) {
    }

    public ValidationException(ValidationError error)
        : this([error]) {
    }
}
