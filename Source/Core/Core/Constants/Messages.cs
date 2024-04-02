namespace DotNetToolbox.Constants;

public static class Messages {
    public const string GenericError = "An error ocurred while executing the operation.";

    public const string ValueCannotBeNull = "The value cannot be null.";
    public static CompositeFormat ValueMustBeOfType { get; } = CompositeFormat.Parse("Expected value to be of type '{0}'. Found: '{1}'.");
    public const string ValueIsInvalid = "The value is invalid.";

    public const string StringCannotBeNullOrEmpty = "The string cannot be null or empty.";
    public const string StringCannotBeNullOrWhiteSpace = "The string cannot be null, empty, or have only white spaces.";
    public const string StringCannotBeEmpty = "If the string is not null, it cannot be empty.";
    public const string StringCannotBeEmptyOrWhiteSpace = "If the string is not null, it cannot be empty or have only white spaces.";

    public const string CollectionCannotBeEmpty = "The collection cannot be empty.";
    public const string CollectionContainsInvalid = "The collection cannot invalid element(s).";
    public const string CollectionContainsNull = "The collection cannot contain null element(s).";
}
