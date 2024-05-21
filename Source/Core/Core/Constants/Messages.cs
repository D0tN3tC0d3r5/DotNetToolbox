namespace DotNetToolbox.Constants;

internal static class Messages {
    internal static CompositeFormat ValueMustBeOfType { get; } = CompositeFormat.Parse("Expected value to be of type '{0}'. Found: '{1}'.");

    public const string ValueIsNotValid = "The value is not valid.";
    public const string CollectionCannotBeEmpty = "The collection cannot be empty.";
    public const string CollectionContainsInvalid = "The collection cannot have invalid element(s).";
    public const string CollectionContainsNull = "The collection cannot contain null element(s).";
}
