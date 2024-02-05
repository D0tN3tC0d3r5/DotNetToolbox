namespace DotNetToolbox.Constants;

public static class Messages {
    public const string GenericError = "An error ocurred while executing the operation.";

    public const string ValueMustBeNull = "The value must be null.";
    public const string ValueMustBeValid = "The value must be valid.";
    public const string ValueIsNotValid = "The value is not valid.";
    public const string ValueMustBeOfType = "The value must be of type '{0}'. Found: '{1}'.";

    public const string ValueMustBeNullOrEmpty = "The value must be null or empty.";
    public const string ValueMustBeNullOrWhiteSpace = "The value must be null or white space.";

    public const string CollectionMustBeEmpty = "The value must be empty.";
    public const string CollectionIsInvalid = "The collection contains invalid elements.";
    public const string ElementAtMustBeNull = "The element at index {0} null.";
    public const string ElementAtMustBeNullOrEmpty = "The element at index {0} or empty.";
    public const string ElementAtMustBeNullOrWhiteSpace = "The element at index {0} must be null or white space.";
    public const string ElementAtMustBeValid = "The element at index {0} must be valid.";
    public const string ElementAtIsNotValid = "The element at index {0} is not valid.";

    public static readonly string ValueCannotBeNull = NegateMessage(ValueMustBeNull);

    public static readonly string ValueCannotBeNullOrEmpty = NegateMessage(ValueMustBeNullOrEmpty);
    public static readonly string ValueCannotBeNullOrWhiteSpace = NegateMessage(ValueMustBeNullOrWhiteSpace);

    public static readonly string CollectionCannotBeEmpty = NegateMessage(CollectionMustBeEmpty);
    public static readonly string ElementAtCannotBeNull = NegateMessage(ElementAtMustBeNull);
    public static readonly string ElementAtCannotBeNullOrEmpty = NegateMessage(ElementAtMustBeNullOrEmpty);
    public static readonly string ElementAtCannotBeNullOrWhiteSpace = NegateMessage(ElementAtMustBeNullOrWhiteSpace);

    internal static string NegateMessage(string message)
        => message switch {
            _ when message.Contains(" cannot ") => message.Replace(" cannot ", " must "),
            _ when message.Contains(" must ") => message.Replace(" must ", " cannot "),
            _ when message.Contains(" is not ") => message.Replace(" is not ", " is "),
            _ when message.Contains(" is ") => message.Replace(" is ", " is not "),
            _ => message,
        };
}
