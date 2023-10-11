namespace System.Constants;

public static class ErrorMessages {
    public static string GetInvertedErrorMessage(string message, params object?[] args)
        => GetErrorMessage(InvertMessage(message), args);

    public static string GetErrorMessage(string message, params object?[] args)
        => string.Format(message, args);

    private static string InvertMessage(string message) => message switch {
        _ when message.Contains(" cannot ") => message.Replace(" cannot ", " must "),
        _ when message.Contains(" must ") => message.Replace(" must ", " cannot "),
        _ when message.Contains(" is not ") => message.Replace(" is not ", " is "),
        _ when message.Contains(" is ") => message.Replace(" is ", " is not "),
        _ => message,
    };

    public static string CannotBeNull => "'{0}' cannot be null.";
    public static string CannotBeNullOrEmpty => "'{0}' cannot be null or empty.";
    public static string CannotBeEmpty => "'{0}' cannot be empty.";
    public static string CannotBeNullOrWhiteSpace => "'{0}' cannot be null or whitespace.";
    public static string CannotContainNull => "'{0}' cannot contain null item(s).";
    public static string CannotContainNullOrEmpty => "'{0}' cannot contain null or empty string(s).";
    public static string CannotContainNullOrWhitespace => "'{0}' cannot contain null or whitespace string(s).";
    public static string MustBeOfType => "'{0}' must be of type '{1}'. Found: '{2}'.";
    public static string MustBe => "'{0}' must be '{1}'. Found: '{2}'.";
}
