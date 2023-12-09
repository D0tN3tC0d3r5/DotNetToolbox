namespace System.Constants;

public static class ErrorMessages {
    public const string MustBeAfter = "Value must be after {0}. Found: {1}.";
    public const string MustBeBefore = "Value must be before {0}. Found: {1}.";
    public const string MustBeEmpty = "Value must be empty.";
    public const string MustBeEmptyOrWhiteSpace = "Value must be empty or white space.";
    public const string MustBeEqualTo = "Value must be equal to {0}. Found: {1}.";
    public const string MustBeGraterThan = "Value must be greater than {0}. Found: {1}.";
    public const string MustBeIn = "Value must be one of these: '{0}'. Found: {1}.";
    public const string MustBeLessThan = "Value must be less than {0}. Found: {1}.";
    public const string MustBeNull = "Value must be null.";
    public const string MustBeNullOrEmpty = "Value must be null or empty.";
    public const string MustBeNullOrWhiteSpace = "Value must be null or white space.";
    public const string MustContain = "Value must contain '{0}'.";
    public const string MustContainValue = "Value must contain the value '{0}'.";
    public const string MustContainKey = "Value must contain the key '{0}'.";
    public const string MustContainNull = "Value must contain null item(s).";
    public const string MustContainNullOrEmpty = "Value must contain null or empty string(s).";
    public const string MustContainNullOrWhiteSpace = "Value must contain null or white space string(s).";
    public const string MustHaveACountOf = "Value count must be {0}. Found: {1}.";
    public const string MustHaveALengthOf = "Value length must be {0}. Found: {1}.";
    public const string MustHaveAMaximumCountOf = "Value maximum count must be {0}. Found: {1}.";
    public const string MustHaveAMaximumLengthOf = "Value maximum length must be {0}. Found: {1}.";
    public const string MustHaveAMinimumCountOf = "Value minimum count must be {0}. Found: {1}.";
    public const string MustHaveAMinimumLengthOf = "Value minimum length must be {0}. Found: {1}.";
    public const string MustBeValid = "Value must be valid.";
    public const string MustBeAValidEmail = "Value must be a valid email.";
    public const string MustBeAValidPassword = "Value must be a valid password.";
    public const string MustBeOfType = "Value must be of type '{0}'. Found: '{1}'.";

    public static readonly string CannotBeAfter = InvertMessage(MustBeAfter);
    public static readonly string CannotBeBefore = InvertMessage(MustBeBefore);
    public static readonly string CannotBeEmpty = InvertMessage(MustBeEmpty);
    public static readonly string CannotBeEmptyOrWhiteSpace = InvertMessage(MustBeEmptyOrWhiteSpace);
    public static readonly string CannotBeEqualTo = InvertMessage(MustBeEqualTo);
    public static readonly string CannotBeGraterThan = InvertMessage(MustBeGraterThan);
    public static readonly string CannotBeIn = InvertMessage(MustBeIn);
    public static readonly string CannotBeLessThan = InvertMessage(MustBeLessThan);
    public static readonly string CannotBeNull = InvertMessage(MustBeNull);
    public static readonly string CannotBeNullOrEmpty = InvertMessage(MustBeNullOrEmpty);
    public static readonly string CannotBeNullOrWhiteSpace = InvertMessage(MustBeNullOrWhiteSpace);
    public static readonly string CannotContain = InvertMessage(MustContain);
    public static readonly string CannotContainValue = InvertMessage(MustContainValue);
    public static readonly string CannotContainKey = InvertMessage(MustContainKey);
    public static readonly string CannotContainNull = InvertMessage(MustContainNull);
    public static readonly string CannotContainNullOrEmpty = InvertMessage(MustContainNullOrEmpty);
    public static readonly string CannotContainNullOrWhiteSpace = InvertMessage(MustContainNullOrWhiteSpace);
    public static readonly string CannotHaveCount = InvertMessage(MustHaveACountOf);
    public static readonly string CannotHaveLength = InvertMessage(MustHaveALengthOf);
    public static readonly string CannotHaveMaximumCount = InvertMessage(MustHaveAMaximumCountOf);
    public static readonly string CannotHaveMaximumLength = InvertMessage(MustHaveAMaximumLengthOf);
    public static readonly string CannotHaveMinimumCount = InvertMessage(MustHaveAMinimumCountOf);
    public static readonly string CannotHaveMinimumLength = InvertMessage(MustHaveAMinimumLengthOf);
    public static readonly string CannotBeOfType = InvertMessage(MustBeOfType);

    public static string InvertMessage(string message)
        => message switch {
            _ when message.Contains(" cannot ") => message.Replace(" cannot ", " must "),
            _ when message.Contains(" must ") => message.Replace(" must ", " cannot "),
            _ when message.Contains(" is not ") => message.Replace(" is not ", " is "),
            _ when message.Contains(" is ") => message.Replace(" is ", " is not "),
            _ => message,
        };
}
