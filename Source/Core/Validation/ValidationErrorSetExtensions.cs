namespace DotNetToolbox.Validation;

public static class ValidationErrorSetExtensions {
    public static bool Contains(this IEnumerable<ValidationError> errors, string message)
        => errors.Contains(string.Empty, message);
    public static bool Contains(this IEnumerable<ValidationError> errors, string source, string message)
        => errors.Contains(new ValidationError(source, message));
}
