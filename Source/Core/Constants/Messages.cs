namespace DotNetToolbox.Constants;

public static class Messages {
    public const string ValueMustBeNull = "Value must be null.";
    public const string ValueMustBeValid = "Value must be valid.";
    public const string ValueIsNotValid = "Value is not valid.";
    public const string ValueMustBeOfType = "Value must be of type '{0}'. Found: '{1}'.";

    public const string ValueMustBeNullOrEmpty = "Value must be null or empty.";
    public const string ValueMustBeNullOrWhiteSpace = "Value must be null or white space.";

    public const string ValueMustBeEmpty = "Value must be empty.";
    public const string ValueMustContainNullItem = "Value must contain null value(s).";
    public const string ValueMustContainEmptyString = "Value must contain null or empty string(s).";
    public const string ValueMustContainWhiteSpaceString = "Value must contain null or white space string(s).";

    public static readonly string ValueCannotBeNull = NegateMessage(ValueMustBeNull);

    public static readonly string ValueCannotBeNullOrEmpty = NegateMessage(ValueMustBeNullOrEmpty);
    public static readonly string ValueCannotBeNullOrWhiteSpace = NegateMessage(ValueMustBeNullOrWhiteSpace);

    public static readonly string ValueCannotBeEmpty = NegateMessage(ValueMustBeEmpty);
    public static readonly string ValueCannotContainNullItem = NegateMessage(ValueMustContainNullItem);
    public static readonly string ValueCannotContainEmptyString = NegateMessage(ValueMustContainEmptyString);
    public static readonly string ValueCannotContainWhiteSpaceString = NegateMessage(ValueMustContainWhiteSpaceString);

    internal static string NegateMessage(string message)
        => message switch {
            _ when message.Contains(" cannot ") => message.Replace(" cannot ", " must "),
            _ when message.Contains(" must ") => message.Replace(" must ", " cannot "),
            _ when message.Contains(" is not ") => message.Replace(" is not ", " is "),
            _ when message.Contains(" is ") => message.Replace(" is ", " is not "),
            _ => message,
        };
}
