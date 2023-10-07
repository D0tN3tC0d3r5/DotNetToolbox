namespace System.Validation;

public static class ValidationErrorCollectionExtensions {
    public static IEnumerable<ValidationError> Merge(this IEnumerable<ValidationError> errors, IEnumerable<ValidationError> otherErrors)
        => errors.Union(otherErrors);

    public static IEnumerable<ValidationError> Merge(this IEnumerable<ValidationError> errors, ValidationError error)
        => errors.Merge(new[] { error });
}
