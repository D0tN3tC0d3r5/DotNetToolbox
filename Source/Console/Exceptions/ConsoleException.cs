namespace DotNetToolbox.ConsoleApplication.Exceptions;

public class ConsoleException(string message, Exception? innerException = null) : Exception(IsNotNullOrEmpty(message), innerException) {
    public const string DefaultMessage = "An error occurred while executing the application.";

    public ConsoleException(int exitCode = IApplication.DefaultErrorCode, string? message = null, Exception? innerException = null)
        : this(message ?? DefaultMessage, innerException) {
        ExitCode = exitCode;
    }

    public int ExitCode { get; }
}
