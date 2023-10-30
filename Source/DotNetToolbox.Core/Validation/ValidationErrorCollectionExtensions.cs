namespace System.Validation;

public static class ValidationErrorCollectionExtensions {
    public static bool Contains(this IEnumerable<ValidationError> errors, string message)
        => errors.Any(error => error.FormattedMessage == message);

    public static void Merge(this ICollection<ValidationError> errors, IEnumerable<ValidationError> otherErrors) {
        foreach (var error in otherErrors.Where(e => !errors.Contains(e)))
            errors.Add(error);
    }

    public static void Merge(this ICollection<ValidationError> errors, ValidationError error)
        => errors.Merge(new[] { error });
}
