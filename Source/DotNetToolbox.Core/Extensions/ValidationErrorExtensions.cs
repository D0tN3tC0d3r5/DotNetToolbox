namespace DotNetToolbox.Extensions;

public static class ValidationErrorCollectionExtensions
{
    public static IEnumerable<IValidationError> Merge(this IEnumerable<IValidationError> errors, IEnumerable<IValidationError> otherErrors)
        => errors.Union(otherErrors);

    public static IEnumerable<IValidationError> Merge(this IEnumerable<IValidationError> errors, IValidationError error)
        => errors.Merge(new [] { error });
}
