namespace System.Validation;

public static class ValidationErrorSetExtensions {
    public static bool Contains(this IEnumerable<ValidationError> errors, string message)
        => errors.Any(error => error.FormattedMessage == message);
}
