namespace DotNetToolbox.Results;

public static class ValidationErrorSetExtensions {
    public static bool Contains(this IEnumerable<ValidationError> errors, string message, string? source = null)
        => errors.Contains(new ValidationError(message, source));
}
